using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using JT.SmartConfigManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Sources
{
    public class AzureKeyVaultSource<T> : LazyConfigSource<T> where T : class, new()
    {
        private readonly string _vaultUri;

        public AzureKeyVaultSource(string vaultUri)
        {
            _vaultUri = vaultUri ?? throw new ArgumentNullException(nameof(vaultUri));
        }

        public override async Task<Dictionary<string, string>> LoadAsync()
        {
            var secrets = new Dictionary<string, string>();
            var client = new SecretClient(new Uri(_vaultUri), new DefaultAzureCredential());

            await foreach (var secretProp in client.GetPropertiesOfSecretsAsync())
            {
                try
                {
                    var secret = await client.GetSecretAsync(secretProp.Name);
                    secrets[secret.Value.Name] = secret.Value.Value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Failed to fetch secret '{secretProp.Name}': {ex.Message}");
                }
            }

            SetValue("VaultSecretsDict", secrets); // <-- This sets it in your strongly typed config

            return secrets;
        }
    }


}
