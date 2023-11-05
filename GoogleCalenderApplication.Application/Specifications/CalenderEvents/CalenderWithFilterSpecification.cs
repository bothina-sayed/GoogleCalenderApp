using GoogleCalenderApplication.Domain.DTOs;
using GoogleCalenderApplication.Domain.Models;
using GoogleCalenderApplication.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Specifications
{
    internal class CalenderWithFilterSpecification : BaseSpecifications<CalenderEvent>
    {
        public CalenderWithFilterSpecification(RequestModel requestModel)
        {
            if (!string.IsNullOrEmpty(requestModel.SummarySearch))
                AddCriteria(x =>x.Summary.Contains(requestModel.SummarySearch));

            if (!string.IsNullOrEmpty(requestModel.DescriptionSearch))
                AddCriteria(x => x.Description.Contains(requestModel.DescriptionSearch));

            if(requestModel.StartDate!=null)
                AddCriteria(x => x.StartDate>=requestModel.StartDate);

            if(requestModel.EndDate!=null)
                AddCriteria(x=> x.EndDate <= requestModel.EndDate);

            ApplyPaging(requestModel.PageSize, requestModel.PageIndex);
        }
    }
}
