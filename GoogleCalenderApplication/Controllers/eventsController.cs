using GoogleCalenderApplication.Application.Abstractions;
using GoogleCalenderApplication.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCalenderApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class eventsController : ControllerBase
    {
        private readonly ICalenderEventService _calenderEventService;

        public eventsController(ICalenderEventService calenderEventService)
        {
            _calenderEventService = calenderEventService;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ResponseModel<CalenderEventDto>>> CreateEvent(CreateCalenderEventDto Dto)
        {
            var result = await _calenderEventService.Add(Dto);
            if (!result.Ok || result ==null)
                return BadRequest(result.Message);

            else
                return CreatedAtAction("GetEvent", new { id = result.Data.EventId }, result.Data);
        }
        [Authorize]
        [HttpGet("{id}", Name = "GetEvent")]
        public IActionResult GetEvent(string id)
        {
            var result = _calenderEventService.GetById(id);

            if (!result.Ok )
            {
                return NotFound(); 
            }
            return Ok(result.Data); 
        }
        [Authorize]
        [HttpDelete("{eventId}", Name = "DeleteEvent")]
        public async Task<ActionResult<ResponseModel<string>>> DeleteEvent(string eventId)
        {
            var result = await _calenderEventService.Delete(eventId);
            if (!result.Ok ) 
                return NotFound();
            else
                return NoContent();
        }
        [Authorize]
        [HttpGet]
        public ActionResult<ResponseModel<List<CalenderEventDto>>> GetEvents([FromQuery]RequestModel requestModel)
           => Ok(_calenderEventService.Get(requestModel ));

    }
}
