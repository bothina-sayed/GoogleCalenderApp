using GoogleCalenderApplication.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Abstractions
{
    public interface ICalenderEventService
    {
        Task<ResponseModel<CalenderEventDto>> Add(CreateCalenderEventDto Dto);
        Task<ResponseModel<string>> Delete(string eventId);
        ResponseModel<CalenderEventDto> GetById(string eventId);
        ResponseModel<List<CalenderEventDto>> Get(RequestModel requestModel);
    }
}
