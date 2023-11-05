using GoogleCalenderApplication.Application.Abstractions;
using GoogleCalenderApplication.Application.Specifications;
using GoogleCalenderApplication.Domain.Abstractions;
using GoogleCalenderApplication.Domain.DTOs;
using GoogleCalenderApplication.Domain.Models;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Services
{
    internal class RefreshTokenService : IRefreshTokenService
    {
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<RefreshToken> _logger;
        private readonly IJWTService _jwtTokenService;
        private readonly IConfiguration _configuration;

        public RefreshTokenService(IGenericRepository<RefreshToken> refreshTokenService,
            IMapper mapper, ILogger<RefreshToken> logger, IJWTService jwtTokenService, IConfiguration configuration)
        {
            _refreshTokenRepo = refreshTokenService;
            _mapper = mapper;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }

        public async Task<ResponseModel<AuthDto>> RefreshToken(string token)
        {
            try
            {
                var result = _refreshTokenRepo
                    .GetEntityWithSpec(new RefreshTokenByTokenSpecification(token));

                if (result.data == null)
                    return ResponseModel<AuthDto>.Error("invalid refresh token");

                if (result.data.RevokedOn.HasValue)
                    return ResponseModel<AuthDto>.Error("inactive refresh token");

                var jwtToken = _jwtTokenService.CreateJwtToken(result.data.User);

                AuthDto authDto;

                if (!result.data.IsExpired)
                {
                    authDto = new AuthDto(jwtToken, result.data,
                        _mapper.Map<UserDto>(result.data.User));
                }
                else
                {
                    result.data.Revoke();

                    _refreshTokenRepo.Update(result.data);

                    var newRefreshToken = GenerateRefreshToken(result.data.UserId);

                    await _refreshTokenRepo.Add(newRefreshToken);

                    await _refreshTokenRepo.Save();

                    authDto = new AuthDto(jwtToken, newRefreshToken,
                        _mapper.Map<UserDto>(result.data.User));
                }

                return ResponseModel<AuthDto>.Success(authDto);
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }
            return ResponseModel<AuthDto>.Error();
        }
        public async Task<ResponseModel<string>> RevokeToken(string token)
        {
            try
            {
                var refreshToken = await _refreshTokenRepo.GetObj(rt => rt.AccessToken == token);

                if (refreshToken == null)
                    return ResponseModel<string>.Error("invalid token");

                refreshToken.Revoke();

                _refreshTokenRepo.Update(refreshToken);

                await _refreshTokenRepo.Save();

                return ResponseModel<string>.Success();
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }

            return ResponseModel<string>.Error();
        }
        public async Task<RefreshToken?> GetRefreshToken(string token)
        {
            try
            {
                var refreshToken = await _refreshTokenRepo.GetObj(x => x.AccessToken == token);

                if (refreshToken == null)
                    return null;

                return refreshToken;
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }
            return null;
        }
        public async Task SaveRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                await _refreshTokenRepo.Add(refreshToken);

                await _refreshTokenRepo.Save();
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }
        }
        public RefreshToken GenerateRefreshToken(string userId)
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return new RefreshToken
            {
                UserId = userId,
                AccessToken = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.Now.AddDays(double.Parse(_configuration["Token:RefreshDurationInDays"])),
            };
        }
        public async Task<ResponseModel<string>> DeleteUserRefreshTokens(string userId)
        {
            try
            {
                var result = _refreshTokenRepo.GetWithSpec(new RefreshTokenByUserIdSpecification(userId));

                _refreshTokenRepo.DeleteRange(result.data);

                await _refreshTokenRepo.Save();

                return ResponseModel<string>.Success();
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }
            return ResponseModel<string>.Error();
        }
    }
}
