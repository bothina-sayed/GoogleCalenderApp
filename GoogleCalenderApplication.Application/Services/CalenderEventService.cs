using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using GoogleCalenderApplication.Domain.Abstractions;
using GoogleCalenderApplication.Domain.DTOs;
using GoogleCalenderApplication.Domain.Models;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleCalenderApplication.Application.Specifications;
using GoogleCalenderApplication.Application.Utils;
using Google.Apis.Calendar.v3.Data;
using GoogleCalenderApplication.Application.Abstractions;
using FluentValidation;
using GoogleCalenderApplication.Application.Validation;
using System.Globalization;

namespace GoogleCalenderApplication.Application.Services
{
    internal class CalenderEventService : ICalenderEventService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<CalenderEvent> _calenderEventRepo;
        private readonly ILogger<CalenderEvent> _logger;
        private readonly IGenericRepository<Token> _tokenRepo;
        private readonly IValidator<CreateCalenderEventDto> _createCalenderEventValidation;

        public CalenderEventService(IMapper mapper, IGenericRepository<CalenderEvent> calenderEventRepo, ILogger<CalenderEvent> logger, IGenericRepository<Token> tokenRepo, IValidator<CreateCalenderEventDto> createCalenderEventValidation)
        {
            _mapper = mapper;
            _calenderEventRepo = calenderEventRepo;
            _logger = logger;
            _tokenRepo = tokenRepo;
            _createCalenderEventValidation = createCalenderEventValidation;
        }
        private CalendarService GetCalendarService(string userId)
        {
            try
            {
                var refreshToken = _tokenRepo.GetEntityWithSpec(new TokenWithUserIdSpecification(userId));

                var token = new TokenResponse
                {
                    RefreshToken = refreshToken.data.refresh_token
                };
                var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
                  new GoogleAuthorizationCodeFlow.Initializer
                  {
                      ClientSecrets = new ClientSecrets
                      {
                          ClientId = "245177443031-0dq9gnmade809hp5jpqj2j69a9ekdb9j.apps.googleusercontent.com",
                          ClientSecret = "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"
                      }

                  }), "user", token);

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials,
                });

                return service;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        } 
        public async Task<ResponseModel<CalenderEventDto>> Add(CreateCalenderEventDto Dto)
        {
            try
            {
                var validationResult = await _createCalenderEventValidation.ValidateAsync(Dto);

                if (!validationResult.IsValid)
                    return ResponseModel<CalenderEventDto>
                        .Error(Helpers.ArrangeValidationErrors(validationResult.Errors));

                var userId = TokenExtractor.GetId();

                var service = GetCalendarService(userId);

                Event newEvent = new Event()
                {
                    Summary = Dto.Summary,
                    Description = Dto.Description,
                    Start = new EventDateTime()
                    {
                        DateTime = Dto.StartDate,
                        TimeZone = "Africa/Cairo"   
                    },
                    End = new EventDateTime()
                    {
                        DateTime = Dto.EndDate,
                        TimeZone = "Africa/Cairo"   
                    },
                };
                EventsResource.InsertRequest insertRequest = service.Events.Insert(newEvent, "primary");
                Event createdEvent = insertRequest.Execute();

                var calenderEvent = _mapper.Map<CalenderEvent>(Dto);
                calenderEvent.UserId = userId;

                calenderEvent.EventId = createdEvent.Id;

                await _calenderEventRepo.Add(calenderEvent);

                await _calenderEventRepo.Save();


                return ResponseModel<CalenderEventDto>.Success(_mapper.Map<CalenderEventDto>(calenderEvent));
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }

            return ResponseModel<CalenderEventDto>.Error();
        }
        public async Task<ResponseModel<string>> Delete(string eventId)
        {
            try
            {
                var userId = TokenExtractor.GetId();
                var service = GetCalendarService (userId);

                EventsResource.GetRequest request = service.Events.Get("primary", eventId);

                Event googleEvent = request.Execute();

                if (googleEvent == null) 
                {
                    return ResponseModel<string>.Error("Invalid Event Id");
                }
                await service.Events.Delete("primary", eventId).ExecuteAsync();

                var calenderEvent = await _calenderEventRepo.GetObj(x=>x.EventId == eventId);
                _calenderEventRepo.Delete(calenderEvent);
                await _calenderEventRepo.Save();

                return ResponseModel<string>.Success("Event Removed Successfully");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseModel<string>.Error();
            }
        }
        public ResponseModel<CalenderEventDto> GetById(string eventId)
        {
            try
            {
                var userId = TokenExtractor.GetId();
                var service = GetCalendarService(userId);
                EventsResource.GetRequest request = service.Events.Get("primary", eventId);

                Event googleEvent = request.Execute();
                CalenderEventDto calenderEventDto = new() 
                {
                    EventId = eventId,
                    Summary = googleEvent.Summary,
                    Description = googleEvent.Description,
                    StartDate = googleEvent.Start.DateTime,
                    EndDate = googleEvent.End.DateTime,
                    UserId = userId
                };
                return ResponseModel<CalenderEventDto>.Success(calenderEventDto);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseModel<CalenderEventDto>.Error();
            }

        }
        public ResponseModel<List<CalenderEventDto>> Get(RequestModel requestModel)
        {
            try
            {
                var userId = TokenExtractor.GetId();
                var service = GetCalendarService(userId);


                EventsResource.ListRequest request = service.Events.List("primary");
                request.TimeMin = requestModel.StartDate;
                request.TimeMax = requestModel.EndDate;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                Events events = request.Execute();

                var list = events.Items.AsQueryable();

                if (requestModel.SummarySearch != null)
                    list = list.Where(x => x.Summary.Contains(requestModel.SummarySearch));
                if (requestModel.DescriptionSearch != null)
                    list = list.Where(x=>x.Description.Contains(requestModel.DescriptionSearch));
                if(requestModel.PageSize > 0 && requestModel.PageIndex > 0)
                    list = list.Skip(requestModel.PageIndex).Take(requestModel.PageSize);

                List<CalenderEventDto> calenderEventDtos = new();
                foreach(var item in list)
                {
                    CalenderEventDto calenderEventDto = new()
                    {
                        Summary = item.Summary,
                        Description = item.Description,
                        StartDate = item.Start.DateTime,
                        EndDate = item.End.DateTime,
                        EventId=item.Id,
                        UserId= TokenExtractor.GetId()

                };
                    calenderEventDtos.Add(calenderEventDto);

                }
                //var result = _calenderEventRepo.GetWithSpec(new CalenderWithFilterSpecification(requestModel));

                //var calenderEvents = _mapper.Map<List<CalenderEventDto>>(result.data);

                return ResponseModel<List<CalenderEventDto>>.Success(calenderEventDtos);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.ToString());
            }
            return ResponseModel<List<CalenderEventDto>>.Error();

        }
    }
}
