using GoogleCalenderApplication.Domain.Models;
using GoogleCalenderApplication.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Specifications
{
    internal class TokenWithUserIdSpecification : BaseSpecifications<Token>
    {
        public TokenWithUserIdSpecification(string userId)
        {
            AddCriteria(x => x.UserId == userId);
        }
    }
}
