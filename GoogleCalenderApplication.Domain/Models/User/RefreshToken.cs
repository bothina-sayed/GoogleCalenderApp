using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Domain.Models
{
    public class RefreshToken : BaseModel
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.Now >= ExpiresOn;
        public DateTime? RevokedOn { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public string UserId { get; set; }

        public void Revoke()
        {
            RevokedOn = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
