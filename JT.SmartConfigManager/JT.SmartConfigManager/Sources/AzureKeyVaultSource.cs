using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Sources
{
    public class AzureKeyVaultSource : IConfigSource
    {
        private readonly string _vaultUrl;
        public AzureKeyVaultSource(string vaultUrl) => _vaultUrl = vaultUrl;

        public Dictionary<string, string> Load()
        {
            var client = new SecretClient(new Uri(_vaultUrl), new DefaultAzureCredential());
            var result = new Dictionary<string, string>();

            foreach (var secretProperties in client.GetPropertiesOfSecrets())
            {
                var secret = client.GetSecret(secretProperties.Name);
                result[secret.Value.Name] = secret.Value.Value;
            }
            return result;
        }
    }
}
