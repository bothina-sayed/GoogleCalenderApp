using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Domain.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<CalenderEvent>? CalenderEvents { get; set; }
        public virtual ICollection<Token> Tokens { get; set;}

    }
}
