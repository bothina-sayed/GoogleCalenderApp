using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Domain.Models
{
    public class Token : BaseModel
    {
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public string UserId { get; set; }
    }
}
