using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Utils
{
    public static class Helpers
    {
        public static string ArrangeValidationErrors(List<ValidationFailure> validationFailures)
        {
            StringBuilder errors = new StringBuilder();

            foreach (var error in validationFailures)
                errors.Append($"{error}\n");

            return errors.ToString();
        }

        public static string ArrangeIdentityErrors(IEnumerable<IdentityError> identityError)
        {
            var errors = new StringBuilder();

            foreach (var error in identityError)
                errors.Append($"{error}\n");

            return errors.ToString();
        }
    }
}
