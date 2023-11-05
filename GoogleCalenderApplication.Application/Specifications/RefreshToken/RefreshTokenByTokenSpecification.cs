using GoogleCalenderApplication.Domain.Models;
using GoogleCalenderApplication.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Specifications
{
    internal class RefreshTokenByTokenSpecification : BaseSpecifications<RefreshToken>
    {
        public RefreshTokenByTokenSpecification(string token)
        {
            AddCriteria(x => x.AccessToken == token);

            AddInclude(new List<string> { $"{nameof(RefreshToken.User)}", });
        }
    }
    
}
