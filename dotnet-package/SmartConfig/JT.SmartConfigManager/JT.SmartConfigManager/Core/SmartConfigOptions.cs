using JT.SmartConfigManager.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Core
{
    public class SmartConfigOptions<T> where T : class, new()
    {
        public List<IConfigSource> Sources { get; } = new();
        public Func<T, bool>? ValidationFunc { get; private set; }
        public string? ValidationError { get; private set; }
        public TimeSpan? AutoReloadInterval { get; private set; }

        public void Validate(Func<T, bool> validator, string errorMessage)
        {
            ValidationFunc = validator;
            ValidationError = errorMessage;
        }

        public void EnableAutoReload(TimeSpan interval) => AutoReloadInterval = interval;

        public void AddJsonFile(string path) => Sources.Add(new JsonFileSource(path));
        public void AddAzureAppConfiguration(string connection) => Sources.Add(new AzureAppConfigSource(connection));
        public void AddAzureKeyVault(string vaultUrl) => Sources.Add(new AzureKeyVaultSource<T>(vaultUrl));
        public void AddSqlConfigStore(string connection, string table) => Sources.Add(new SqlConfigSource(connection, table));
    }

}
