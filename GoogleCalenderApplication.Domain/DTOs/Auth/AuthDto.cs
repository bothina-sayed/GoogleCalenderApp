using GoogleCalenderApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Domain.DTOs
{
    public class AuthDto
    {
        public AuthDto(JwtSecurityToken jwtToken, RefreshToken refreshToken, UserDto user, string? URL="")
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            ExpiresOn = jwtToken.ValidTo;
            User = user;
            RefreshToken = refreshToken.AccessToken;
            RefreshTokenExpiration = refreshToken.ExpiresOn;
            GoogleRedirectURL = URL;
        }

        public UserDto User { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public string? GoogleRedirectURL { get; set; }
    }
}
