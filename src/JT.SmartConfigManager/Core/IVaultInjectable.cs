using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Core
{
    public interface IVaultInjectable
    {
        Dictionary<string, string>? VaultSecretsDict { get; set; }
    }
}
