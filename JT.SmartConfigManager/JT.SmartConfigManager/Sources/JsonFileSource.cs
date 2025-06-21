using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Sources
{
    public class JsonFileSource : IConfigSource
    {
        private readonly string _path;
        public JsonFileSource(string path) => _path = path;

        public Dictionary<string, string> Load()
        {
            var config = new ConfigurationBuilder().AddJsonFile(_path).Build();
            return config.AsEnumerable()
                         .Where(kv => kv.Value != null)
                         .ToDictionary(kv => kv.Key, kv => kv.Value!);
        }
    }
}
