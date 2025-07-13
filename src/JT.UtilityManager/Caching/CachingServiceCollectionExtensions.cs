using JT.UtilityManager.Caching.Interfaces;
using JT.UtilityManager.Caching.Services;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.UtilityManager.Caching
{
    public static class CachingServiceCollectionExtensions
    {
        /// <summary>
        /// Registers in-memory caching and related services.
        /// </summary>
        public static IServiceCollection AddInMemoryCaching(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IInMemoryCacheService, InMemoryCacheService>();
            return services;
        }

        /// <summary>
        /// Registers distributed Redis caching using custom configuration.
        /// </summary>
        public static IServiceCollection AddRedisCaching(this IServiceCollection services, Action<RedisCacheOptions> configure)
        {
            services.AddStackExchangeRedisCache(configure);
            services.AddScoped<IDistributedCacheService, RedisCacheService>();
            return services;
        }

        /// <summary>
        /// Registers distributed Redis caching using configuration from IConfiguration.
        /// Expects "Redis:ConnectionString" in appsettings.json or environment.
        /// </summary>
        public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConn = configuration["Redis:ConnectionString"];

            if (string.IsNullOrWhiteSpace(redisConn))
                throw new ArgumentNullException("Redis:ConnectionString", "Redis connection string is not configured.");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConn;
            });

            services.AddScoped<IDistributedCacheService, RedisCacheService>();
            return services;
        }
    }
}
