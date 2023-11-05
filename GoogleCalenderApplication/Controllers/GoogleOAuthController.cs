using GoogleCalenderApplication.Application.Abstractions;
using GoogleCalenderApplication.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCalenderApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleOAuthController : ControllerBase
    {
        private readonly IGoogleOAuthService _googleOAuthService;

        public GoogleOAuthController(IGoogleOAuthService googleOAuthService)
        {
            _googleOAuthService = googleOAuthService;
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseModel<AuthDto>>> RefreshToken(string token)
            => Ok(await _googleOAuthService.GetNewRefreshToken(token));

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseModel<string>>> RevokeToken(string token)
            => Ok(await _googleOAuthService.RevokeToken(token));
    }
}
