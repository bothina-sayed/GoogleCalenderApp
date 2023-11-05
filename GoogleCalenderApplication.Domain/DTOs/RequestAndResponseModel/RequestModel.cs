using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Domain.DTOs
{
    public class RequestModel
    {
        public string? SummarySearch { get; set; }
        public string? DescriptionSearch { get; set; }
        public DateTime? StartDate {  get; set; }    
        public DateTime? EndDate { get; set; }
        public int PageIndex { get; set; } 
        public int PageSize { get; set; } 
    }
}
