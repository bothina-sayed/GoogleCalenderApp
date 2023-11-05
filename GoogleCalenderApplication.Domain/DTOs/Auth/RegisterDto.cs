using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Domain.DTOs
{
    public class RegisterDto
    {
        public string FullName { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string UserName { get; init; }
        public string Password { get; init; }
    }
}
