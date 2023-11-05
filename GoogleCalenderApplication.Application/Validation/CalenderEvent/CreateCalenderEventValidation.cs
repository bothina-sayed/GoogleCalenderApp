using FluentValidation;
using GoogleCalenderApplication.Domain.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Validation
{
    internal class CreateCalenderEventValidation : AbstractValidator<CreateCalenderEventDto>
    {
        public CreateCalenderEventValidation()
        {
            RuleFor(u => new { u.StartDate, u.EndDate })
                .NotEmpty().MustAsync((a, cancellationToken) => isDatesValid(a.StartDate, a.EndDate, cancellationToken)).WithMessage("Please Check Dates Again");
            
            RuleFor(x => x.StartDate)
                .NotEmpty().MustAsync(isDateInPast).WithMessage("You Can't Create Event in the Past");

            RuleFor(x => x.StartDate)
                .NotEmpty().MustAsync(isDateIsHoliday).WithMessage("You Can't Create Event Friday’s or Saturday’s");

        }
        private async Task<bool> isDateInPast(DateTime startDate,CancellationToken cancellationToken)
        {
            if (startDate < DateTime.Now )
                return false;

            return true;
        }
        private async Task<bool> isDateIsHoliday(DateTime startDate, CancellationToken cancellationToken)
        {
            if (startDate.DayOfWeek == DayOfWeek.Friday || startDate.DayOfWeek == DayOfWeek.Saturday)
                return false;

            return true;
        }

        private async Task<bool> isDatesValid(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            if (startDate > endDate)
                return false;
            return true;

        }
    }
}
