using JT.SmartConfigManager.Sources;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Sources
{
    public class AzureAppConfigSource : IConfigSource
    {
        private readonly string _connection;
        public AzureAppConfigSource(string connection) => _connection = connection;

        public Task<Dictionary<string, string>> LoadAsync()
        {
            var config = new ConfigurationBuilder()
                .AddAzureAppConfiguration(_connection)
                .Build();

            var dict = config.AsEnumerable()
                             .Where(kv => kv.Value != null)
                             .ToDictionary(kv => kv.Key, kv => kv.Value!);

            return Task.FromResult(dict);
        }
    }
}

