using FluentAssertions;
using JT.UtilityManager.Caching.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace JT.UtilityManager.Tests.CachedTest
{
    public class MemoryCacheServiceTests
    {
        private readonly InMemoryCacheService _cacheService;

        public MemoryCacheServiceTests()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            _cacheService = new InMemoryCacheService(memoryCache);
        }

        [Fact]
        public async Task SetAsync_Should_Store_And_GetAsync_Should_Return_Value()
        {
            // Arrange
            var key = "my-key";
            var expected = "my-value";
            var expiry = TimeSpan.FromMinutes(2);

            // Act
            await _cacheService.SetAsync(key, expected, expiry);
            var result = await _cacheService.GetAsync<string>(key);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public async Task GetAsync_Should_Return_Null_When_Key_Not_Found()
        {
            // Act
            var result = await _cacheService.GetAsync<string>("missing-key");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_Should_Remove_Cached_Value()
        {
            // Arrange
            var key = "to-remove";
            var value = "value";
            await _cacheService.SetAsync(key, value, TimeSpan.FromMinutes(1));

            // Act
            await _cacheService.RemoveAsync(key);
            var result = await _cacheService.GetAsync<string>(key);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Value_Should_Expire_After_Timeout()
        {
            // Arrange
            var key = "expire-key";
            var value = "expiring-value";
            await _cacheService.SetAsync(key, value, TimeSpan.FromMilliseconds(200));

            // Wait for expiry
            await Task.Delay(300);

            // Act
            var result = await _cacheService.GetAsync<string>(key);

            // Assert
            result.Should().BeNull();
        }
    }
}
