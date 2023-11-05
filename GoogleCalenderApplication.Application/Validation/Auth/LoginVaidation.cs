using FluentValidation;
using GoogleCalenderApplication.Domain.Abstractions;
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
    internal class LoginVaidation : AbstractValidator<LoginDto>
    {
        private readonly UserManager<User> _userManager;

        public LoginVaidation(UserManager<User> userManager)
        {

            RuleFor(u => new { u.Email, u.Password })
                .NotEmpty().MustAsync((a, cancellationToken) => isUserExist(a.Email, a.Password, cancellationToken)).WithMessage("Wrong Email or Password");
            _userManager = userManager;
        }
        private async Task<bool> isUserExist(string email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null ||
                (user != null && !await _userManager.CheckPasswordAsync(user, password)))
                return false;
            return true;

        }


    }

}
