using GoogleCalenderApplication.Domain.Models;
using GoogleCalenderApplication.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Specifications
{
    internal class UserSpecification : BaseSpecifications<User>
    {
        public UserSpecification(string email)
        {
            AddCriteria(x => x.Email == email);

            
        }
    }
}
