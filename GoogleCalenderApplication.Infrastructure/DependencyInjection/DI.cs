using GoogleCalenderApplication.Domain.Abstractions;
using GoogleCalenderApplication.Infrastructure.GenericRepository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Infrastructure.DependencyInjection
{
    public static class DI
    {
        public static void InfrastructureStrapping(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }
    }
}
