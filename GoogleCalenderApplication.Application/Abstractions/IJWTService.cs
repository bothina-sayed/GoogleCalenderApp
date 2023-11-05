using GoogleCalenderApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Abstractions
{
    public interface IJWTService
    {
        JwtSecurityToken CreateJwtToken(User user);
    }
}
