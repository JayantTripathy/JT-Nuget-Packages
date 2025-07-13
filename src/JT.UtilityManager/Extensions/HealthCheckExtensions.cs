using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.UtilityManager.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddJTHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks();
            return services;
        }
    }
}
