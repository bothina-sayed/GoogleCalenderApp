using GoogleCalenderApplication.Application.Abstractions;
using GoogleCalenderApplication.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCalenderApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IGoogleOAuthService _googleOAuthService;

        public AuthController(IAuthService authService, IGoogleOAuthService googleOAuthService)
        {
            _authService = authService;
            _googleOAuthService = googleOAuthService;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseModel<AuthDto>>> Register(RegisterDto registerDto)
            => Ok(await _authService.Register(registerDto, HttpContext));

        [HttpGet]
        [Route("/auth/callback")]
        public async Task<IActionResult> Callback()
        {

            string code = HttpContext.Request.Query["code"];

            if (HttpContext.Request.Cookies.TryGetValue("UserEmail", out string redirectUrl))
            {
                var email = redirectUrl;
                HttpContext.Response.Cookies.Delete("RedirectAfterOAuth");

                var token = await _googleOAuthService.GetToken(code, email);
                return Ok(token);
            }
            else
                return BadRequest();

        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseModel<AuthDto>>> Login(LoginDto loginDto)
            => Ok(await _authService.Login(loginDto));
    }
}
