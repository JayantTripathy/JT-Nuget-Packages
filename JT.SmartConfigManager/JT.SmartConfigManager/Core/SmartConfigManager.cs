using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Core
{
    public class SmartConfigManager<T> where T : class, new()
    {
        private readonly SmartConfigOptions<T> _options;
        private T _current = new();
        private Timer? _timer;

        public T Current => _current;

        public SmartConfigManager(SmartConfigOptions<T> options)
        {
            _options = options;
            Load();

            if (options.AutoReloadInterval.HasValue)
            {
                _timer = new Timer(_ => Load(), null, options.AutoReloadInterval.Value, options.AutoReloadInterval.Value);
            }
        }

        private void Load()
        {
            var merged = new Dictionary<string, string>();
            foreach (var source in _options.Sources)
            {
                var data = source.Load();
                foreach (var kv in data)
                    merged[kv.Key] = kv.Value;
            }

            var config = new ConfigurationBuilder().AddInMemoryCollection(merged).Build();
            var bound = new T();
            config.Bind(bound);

            if (_options.ValidationFunc != null && !_options.ValidationFunc(bound))
                throw new Exception(_options.ValidationError);

            _current = bound;
        }

        public void Start() => Load();
    }
}
