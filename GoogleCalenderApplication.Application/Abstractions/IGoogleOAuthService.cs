using GoogleCalenderApplication.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Abstractions
{
    public interface IGoogleOAuthService
    {
        string GetAuthCode(HttpContext httpContext, string email);
        Task<ResponseModel<ResponseTokenDto>> GetToken(string code, string email);
        Task<ResponseModel<string>> RevokeToken(string refeshToken, string appRefreshToken);
        Task<ResponseModel<ResponseTokenDto>> GetNewRefreshToken(string refeshToken, string appRefreshToken);
    }
}
