using GoogleCalenderApplication.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Abstractions
{
    public interface IAuthService
    {
        Task<ResponseModel<AuthDto>> Login(LoginDto loginDto);
        Task<ResponseModel<AuthDto>> Register(RegisterDto registerDto, HttpContext httpContext);
    }
}
