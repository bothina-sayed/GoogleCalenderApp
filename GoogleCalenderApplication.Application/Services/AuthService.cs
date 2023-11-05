using FluentValidation;
using GoogleCalenderApplication.Application.Abstractions;
using GoogleCalenderApplication.Application.Specifications;
using GoogleCalenderApplication.Application.Utils;
using GoogleCalenderApplication.Domain.Abstractions;
using GoogleCalenderApplication.Domain.DTOs;
using GoogleCalenderApplication.Domain.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IValidator<LoginDto> _loginValidation;
        private readonly IValidator<RegisterDto> _registerValidation;
        private readonly IMapper _mapper;
        private readonly ILogger<User> _logger;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IJWTService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IGoogleOAuthService _googleOAuthService;

        public AuthService(UserManager<User> userManager,
            IValidator<LoginDto> loginValidation, IValidator<RegisterDto> registerValidation,
            IMapper mapper, ILogger<User> logger, IGenericRepository<User> userRepo,
            IJWTService jwtTokenService, IRefreshTokenService refreshTokenService, IGoogleOAuthService googleOAuthService)
        {
            _userManager = userManager;
            _loginValidation = loginValidation;
            _registerValidation = registerValidation;
            _mapper = mapper;
            _logger = logger;
            _userRepo = userRepo;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
            _googleOAuthService = googleOAuthService;
        }

        public async Task<ResponseModel<AuthDto>> Login(LoginDto loginDto)
        {
            try
            {
                var validationResult = await _loginValidation.ValidateAsync(loginDto);

                if (!validationResult.IsValid)
                    return ResponseModel<AuthDto>
                        .Error(Helpers.ArrangeValidationErrors(validationResult.Errors));

                if (!string.IsNullOrEmpty(loginDto.RefreshToken))
                    await _refreshTokenService.RevokeToken(loginDto.RefreshToken);

                var user = _userRepo.GetEntityWithSpec(
                    new UserSpecification(loginDto.Email)).data;


                await _userRepo.Save();

                var jwtToken = _jwtTokenService.CreateJwtToken(user);

                var newRefreshToken = _refreshTokenService.GenerateRefreshToken(user.Id);

                await _refreshTokenService.SaveRefreshToken(newRefreshToken);


                var authDto = new AuthDto(jwtToken, newRefreshToken,
                    _mapper.Map<UserDto>(user));

                return ResponseModel<AuthDto>.Success(authDto);
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }

            return ResponseModel<AuthDto>.Error();
        }

        public async Task<ResponseModel<AuthDto>> Register(RegisterDto registerDto , HttpContext httpContext)
        {
            try
            {
                var validationResult = await _registerValidation.ValidateAsync(registerDto);

                if (!validationResult.IsValid)
                    return ResponseModel<AuthDto>
                        .Error(Helpers.ArrangeValidationErrors(validationResult.Errors));

                var user = _mapper.Map<User>(registerDto);

                var googleResult = _googleOAuthService.GetAuthCode(httpContext,user.Email);

                if (googleResult == null)
                    return ResponseModel<AuthDto>.Error();

                var createResult = await _userManager.CreateAsync(user, registerDto.Password);

                if (!createResult.Succeeded)
                    return ResponseModel<AuthDto>
                        .Error(Helpers.ArrangeIdentityErrors(createResult.Errors));

                var jwtToken = _jwtTokenService.CreateJwtToken(user);

                var refreshToken = _refreshTokenService.GenerateRefreshToken(user.Id);

                await _refreshTokenService.SaveRefreshToken(refreshToken);



                var authDto = new AuthDto(jwtToken, refreshToken,
                        _mapper.Map<UserDto>(user) , googleResult);

                return ResponseModel<AuthDto>.Success(authDto);
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }

            return ResponseModel<AuthDto>.Error();
        }
    }
}
