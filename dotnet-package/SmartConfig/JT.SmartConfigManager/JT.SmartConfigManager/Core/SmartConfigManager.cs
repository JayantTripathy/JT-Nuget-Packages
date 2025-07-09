using JT.SmartConfigManager.Sources;
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

        // ✅ Expose final merged config dictionary
        public Dictionary<string, string>? AllSettings { get; private set; }
        public Dictionary<string, string>? VaultSecrets { get; private set; } // ✅ Add this


        public T Current => _current;

        public SmartConfigManager(SmartConfigOptions<T> options)
        {
            _options = options;
            LoadAsync().Wait(); // sync wait on constructor

            if (_options.AutoReloadInterval.HasValue)
            {
                _timer = new Timer(_ => LoadAsync(), null, _options.AutoReloadInterval.Value, _options.AutoReloadInterval.Value);
            }
        }

        private async Task LoadAsync()
        {
            var merged = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var vaultOnly = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var source in _options.Sources)
            {
                var data = await source.LoadAsync();

                if (source is AzureKeyVaultSource<T>)
                {
                    foreach (var kv in data)
                        vaultOnly[kv.Key] = kv.Value!;
                }

                foreach (var kv in data)
                    merged[kv.Key] = kv.Value!;
            }

            AllSettings = merged;
            VaultSecrets = vaultOnly;

            var config = new ConfigurationBuilder().AddInMemoryCollection(merged).Build();
            var bound = new T();
            config.Bind(bound);

            // ✅ Inject vault secrets if supported
            if (bound is IVaultInjectable injectable)
                injectable.VaultSecretsDict = vaultOnly;

            if (_options.ValidationFunc != null && !_options.ValidationFunc(bound))
                throw new Exception(_options.ValidationError);

            _current = bound;
        }



        public void Start() => LoadAsync().Wait();
    }
}
