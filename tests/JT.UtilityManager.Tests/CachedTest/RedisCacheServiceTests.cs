using JT.UtilityManager.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JT.UtilityManager.Tests.CachedTest
{
    public class RedisCacheServiceTests
    {
        [Fact]
        public async Task SetAndGetAsync_WorksCorrectly()
        {
            // Arrange
            var mockCache = new Mock<IDistributedCache>();
            var redisService = new RedisCacheService(mockCache.Object);
            var key = "testKey";
            var value = new TestObject { Name = "RedisTest" };
            var json = JsonSerializer.Serialize(value);
            var bytes = Encoding.UTF8.GetBytes(json);

            mockCache.Setup(c => c.GetAsync(key, It.IsAny<CancellationToken>())).ReturnsAsync(bytes);

            // Act
            await redisService.SetAsync(key, value, TimeSpan.FromMinutes(5));
            var result = await redisService.GetAsync<TestObject>(key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("RedisTest", result!.Name);
        }

        [Fact]
        public async Task RemoveAsync_RemovesFromCache()
        {
            // Arrange
            var mockCache = new Mock<IDistributedCache>();
            var redisService = new RedisCacheService(mockCache.Object);
            var key = "testKey";

            // Act
            await redisService.RemoveAsync(key);

            // Assert
            mockCache.Verify(c => c.RemoveAsync(key, It.IsAny<CancellationToken>()), Times.Once);
        }

        public class TestObject
        {
            public string? Name { get; set; }
        }
    }
}