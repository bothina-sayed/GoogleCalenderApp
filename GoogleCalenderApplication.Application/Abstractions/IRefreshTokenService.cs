using GoogleCalenderApplication.Domain.DTOs;
using GoogleCalenderApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Abstractions
{
    public interface IRefreshTokenService
    {
        Task<ResponseModel<AuthDto>> RefreshToken(string token);
        Task<ResponseModel<string>> RevokeToken(string token);
        RefreshToken GenerateRefreshToken(string userId);
        Task<RefreshToken?> GetRefreshToken(string token);
        Task SaveRefreshToken(RefreshToken refreshToken);
        Task<ResponseModel<string>> DeleteUserRefreshTokens(string userId);
    }
}
