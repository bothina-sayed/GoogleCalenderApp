using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Domain.Models
{
    public class CalenderEvent : BaseModel
    {
        public string EventId { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public string UserId { get; set; }
    }
}
