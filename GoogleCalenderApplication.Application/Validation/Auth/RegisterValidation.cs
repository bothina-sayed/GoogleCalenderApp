using FluentValidation;
using GoogleCalenderApplication.Domain.DTOs;
using GoogleCalenderApplication.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Validation
{
    internal class RegisterValidation : AbstractValidator<RegisterDto>
    {
        private readonly UserManager<User> _userManager;
        public RegisterValidation(UserManager<User> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress().Must(isEmailUsed).WithMessage("email already in use");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().Must(isPhoneNumberUsed).WithMessage("phone number already in use");

            RuleFor(x => x.UserName)
                .NotEmpty().Must(isUserNameUsed).WithMessage("userName already in use");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("password is required");
        }

        private bool isEmailUsed(string email)
            => !_userManager.Users.Any(x => x.Email == email);

        private bool isPhoneNumberUsed(string userNam)
            => !_userManager.Users.Any(x => x.UserName == userNam);

        private bool isUserNameUsed(string userNam)
            => !_userManager.Users.Any(x => x.UserName == userNam);
    }
}
