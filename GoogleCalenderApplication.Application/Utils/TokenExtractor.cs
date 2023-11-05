using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Utils
{
    public static class TokenExtractor
    {
        public static string GetId()
        {
            var userIdentity = GetHttpContext().User.Identity as ClaimsIdentity;

            var userId = userIdentity!.FindFirst(ClaimTypes.Sid)!.Value;

            return userId;
        }

        public static string GetEmail()
        {
            var userIdentity = GetHttpContext().User.Identity as ClaimsIdentity;

            var userEmail = userIdentity!.FindFirst(ClaimTypes.Email)!.Value;

            return userEmail;
        }
        public static string GetUserName()
        {
            var userIdentity = GetHttpContext().User.Identity as ClaimsIdentity;

            var userName = userIdentity!.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            return userName;
        }
        public static string GetPhoneNumber()
        {
            var userIdentity = GetHttpContext().User.Identity as ClaimsIdentity;

            var phoneNumber = userIdentity!.FindFirst(ClaimTypes.MobilePhone)!.Value;

            return phoneNumber;
        }
        public static HttpContext GetHttpContext()
        {
            var httpContextAccessor = new HttpContextAccessor();
            return httpContextAccessor.HttpContext;
        }
    }
}
