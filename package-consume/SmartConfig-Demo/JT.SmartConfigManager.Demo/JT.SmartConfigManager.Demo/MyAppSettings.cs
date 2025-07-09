using JT.SmartConfigManager.Core;


public class MyAppSettings : IVaultInjectable
{
    public AppSettings? AppSettings { get; set; }
    public SqlConfig? SqlConfig { get; set; }

    public AzureAppConfigConfig? AzureAppConfig { get; set; }
    public Dictionary<string, string>? VaultSecretsDict { get; set; }

    public List<KeyValuePair<string, string>>? VaultSecrets =>
        VaultSecretsDict?.ToList();

    public Dictionary<string, string>? AppConfigDict { get; set; }

    public List<KeyValuePair<string, string>>? AppConfigList { get; set; }
}

public class AppSettings
{
    public string? AppName { get; set; }
    public string? Environment { get; set; }
}

public class SqlConfig
{
    public string ConnectionString { get; set; } = string.Empty;
    public string Query { get; set; } = string.Empty;
}
public class AzureAppConfigConfig
{
    public string? ConnectionString { get; set; }
}

