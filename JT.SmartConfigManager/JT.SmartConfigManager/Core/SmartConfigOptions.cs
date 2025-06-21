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
        internal List<IConfigSource> Sources { get; } = new();
        internal Func<T, bool>? ValidationFunc { get; private set; }
        internal string? ValidationError { get; private set; }
        internal TimeSpan? AutoReloadInterval { get; private set; }

        public void Validate(Func<T, bool> validator, string errorMessage)
        {
            ValidationFunc = validator;
            ValidationError = errorMessage;
        }

        public void EnableAutoReload(TimeSpan interval) => AutoReloadInterval = interval;

        public void AddJsonFile(string path) => Sources.Add(new JsonFileSource(path));
        public void AddAzureAppConfiguration(string connection) => Sources.Add(new AzureAppConfigSource(connection));
        public void AddAzureKeyVault(string vaultUrl) => Sources.Add(new AzureKeyVaultSource(vaultUrl));
        public void AddSqlConfigStore(string connection, string table) => Sources.Add(new SqlConfigSource(connection, table));
    }

}
