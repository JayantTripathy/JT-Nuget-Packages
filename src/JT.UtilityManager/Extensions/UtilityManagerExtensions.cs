using JT.UtilityManager.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.UtilityManager.Extensions
{
    public static class UtilityManagerExtensions
    {
        public static IServiceCollection AddUtilityManager(this IServiceCollection services, IConfiguration config)
        {
            // Load modular services
            services.AddInMemoryCaching();
            services.AddRedisCaching(config);

            // Future additions
            // services.AddLoggingSupport();
            // services.AddFileConverters();

            return services;
        }
    }
}