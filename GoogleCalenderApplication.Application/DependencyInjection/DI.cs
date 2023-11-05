using FluentValidation;
using GoogleCalenderApplication.Application.Abstractions;
using GoogleCalenderApplication.Application.Services;
using GoogleCalenderApplication.Application.Validation;
using GoogleCalenderApplication.Domain.DTOs;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.DependencyInjection
{
    public static class DI
    {
        public static IServiceCollection ApplicationStrapping(this IServiceCollection services)
        {

            #region Services

            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGoogleOAuthService, GoogleOAuthService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<ICalenderEventService, CalenderEventService>();

            #endregion

            #region Validation

            services.AddScoped<IValidator<LoginDto>, LoginVaidation>();
            services.AddScoped<IValidator<RegisterDto>, RegisterValidation>();
            services.AddScoped<IValidator<CreateCalenderEventDto>, CreateCalenderEventValidation>();
            #endregion

            #region Mapster

            var config = TypeAdapterConfig.GlobalSettings;

            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);

            services.AddScoped<IMapper, ServiceMapper>();

            #endregion

            return services;
        }
    }
}
