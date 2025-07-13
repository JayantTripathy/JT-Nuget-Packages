using JT.SmartConfigManager.Core;
using JT.SmartConfigManager.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Extensions
{
    public static class SmartConfigOptionsExtensions
    {
        public static void AddAzureKeyVault<T>(this SmartConfigOptions<T> options, string vaultUrl)
            where T : class, new()
        {
            options.Sources.Add(new AzureKeyVaultSource<T>(vaultUrl));
        }
    }

}
