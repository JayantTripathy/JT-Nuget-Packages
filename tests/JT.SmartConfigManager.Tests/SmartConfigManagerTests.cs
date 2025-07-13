using FluentAssertions;
using JT.SmartConfigManager.Core;
using JT.SmartConfigManager.Sources;

namespace JT.SmartConfigManager.Tests
{
    public class SmartConfigManagerTests
    {
        [Fact]
        public async Task Should_Load_Config_From_Json()
        {
            // Arrange
            var json = @"
            {
                ""AppSettings"": {
                    ""AppName"": ""UnitTest App"",
                    ""Environment"": ""Test""
                }
            }";

            var tempFilePath = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFilePath, json);

            var options = new SmartConfigOptions<MyAppSettings>();
            options.Sources.Add(new JsonFileSource(tempFilePath));

            // Act
            var manager = new SmartConfigManager<MyAppSettings>(options);
            var settings = manager.Current;

            // Assert
            settings.Should().NotBeNull();
            settings.AppSettings.Should().NotBeNull();
            settings.AppSettings.AppName.Should().Be("UnitTest App");
            settings.AppSettings.Environment.Should().Be("Test");

            File.Delete(tempFilePath); // Cleanup
        }
    }

    public class MyAppSettings : IVaultInjectable
    {
        public AppSettings? AppSettings { get; set; }
        public Dictionary<string, string>? VaultSecretsDict { get; set; }
        public List<KeyValuePair<string, string>>? VaultSecrets => VaultSecretsDict?.ToList();
    }

    public class AppSettings
    {
        public string? AppName { get; set; }
        public string? Environment { get; set; }
    }
}
