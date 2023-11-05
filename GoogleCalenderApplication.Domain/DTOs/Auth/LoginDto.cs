using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Domain.DTOs
{
    public class LoginDto
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string? RefreshToken { get; init; }
    }
}
