using JT.UtilityManager.Demo.Models;
using JT.UtilityManager.Demo.Services;
using JT.UtilityManager.Caching.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JT.UtilityManager.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IInMemoryCacheService _memoryCache;
        private readonly IDistributedCacheService _redisCache;
        private readonly ProductService _productService;

        public CacheController(IInMemoryCacheService memoryCache, IDistributedCacheService redisCache, ProductService productService)
        {
            _memoryCache = memoryCache;
            _redisCache = redisCache;
            _productService = productService;
        }

        [HttpGet("memory/{id}")]
        public async Task<IActionResult> GetFromMemory(int id)
        {
            var cacheKey = $"product_memory_{id}";
            var cached = await _memoryCache.GetAsync<Product>(cacheKey);
            if (cached is not null) return Ok(cached);

            var product = _productService.GetProduct(id);
            await _memoryCache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(5));
            return Ok(product);
        }

        [HttpGet("redis/{id}")]
        public async Task<IActionResult> GetFromRedis(int id)
        {
            var cacheKey = $"product_redis_{id}";
            var cached = await _redisCache.GetAsync<Product>(cacheKey);
            if (cached is not null) return Ok(cached);

            var product = _productService.GetProduct(id);
            await _redisCache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(10));
            return Ok(product);
        }
    }
}
