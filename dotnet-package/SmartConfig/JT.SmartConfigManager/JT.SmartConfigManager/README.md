# JT.SmartConfigManager

**JT.SmartConfigManager** is a powerful, unified configuration manager for .NET apps.  
It lets you load and merge configurations from multiple sources like:

- ✅ `appsettings.json`
- ✅ **Azure Key Vault**
- ✅ **Azure App Configuration**
- ✅ **SQL Server Config Table**
- ✅ Environment Variables

All sources are dynamically merged into a **single strongly-typed class** — simplifying config access and management.

---

## 🚀 Why Use This?

Managing configuration across environments can be painful.  
This package solves that by:

- 🔐 Centralizing secrets from multiple sources
- 📦 Auto-binding values into a POCO class
- ⚙️ Supporting validation & auto-reload
- 🧹 Reducing manual code for loading secrets or DB config

It’s ideal for enterprise, cloud-native, or microservice .NET Core apps.

---

## 🔧 Features

- ✅ Merges JSON, SQL, KeyVault, App Config into one model
- ✅ Supports secret injection via `IVaultInjectable`
- ✅ Clean POCO model binding
- ✅ Auto reload on interval (optional)
- ✅ Easily extendable for custom sources

---

## 📦 Install via NuGet

```bash
dotnet add package JT.SmartConfigManager

```


📁 Sample appsettings.json

```json
{
  "AppSettings": {
    "AppName": "Demo API",
    "Environment": "Development"
  },
  "SqlConfig": {
    "ConnectionString": "Server=localhost\\SQLEXPRESS01;Database=testDB;Trusted_Connection=True;TrustServerCertificate=True;",
    "Query": "SELECT [Key], [Value] FROM AppConfig"
  },
  "KeyVault": {
    "VaultUri": "https://jt-configmanager.vault.azure.net/"
  },
  "AzureAppConfig": {
    "ConnectionString": "Endpoint=https://jt-config-manager.azconfig.io;Id=...;Secret=..."
  }
}
```

🧱 POCO Model Example

```csharp   
public class MyAppSettings : IVaultInjectable
{
    public AppSettings? AppSettings { get; set; }
    public SqlConfig? SqlConfig { get; set; }

    public Dictionary<string, string>? VaultSecretsDict { get; set; }

    public List<KeyValuePair<string, string>>? VaultSecrets =>
        VaultSecretsDict?.ToList();

    public List<KeyValuePair<string, string>>? AppConfigList { get; set; }  // Optional for Azure AppConfig
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
```

🛠️ Program.cs Setup
```csharp
var options = new SmartConfigOptions<MyAppSettings>();

options.Sources.Add(new JsonFileSource("appsettings.json"));

if (!string.IsNullOrWhiteSpace(appSettings.SqlConfig?.ConnectionString))
{
    options.Sources.Add(new SqlConfigSource(
        appSettings.SqlConfig.ConnectionString,
        appSettings.SqlConfig.Query));
}

if (!string.IsNullOrWhiteSpace(appSettings.AzureAppConfig?.ConnectionString))
{
    options.Sources.Add(new AzureAppConfigSource(appSettings.AzureAppConfig.ConnectionString));
}

if (!string.IsNullOrWhiteSpace(baseConfig["KeyVault:VaultUri"]))
{
    options.Sources.Add(new AzureKeyVaultSource<MyAppSettings>(baseConfig["KeyVault:VaultUri"]!));
}

var configManager = new SmartConfigManager<MyAppSettings>(options);
var finalSettings = configManager.Current;

builder.Services.AddSingleton(finalSettings);
```

🔍 Example /config Output

The /config endpoint will return a clean JSON like this:


```json
{
  "appName": "Demo API",
  "environment": "Development",
  "sqlConn": "Server=localhost\\SQLEXPRESS01;Database=testDB;Trusted_Connection=True;TrustServerCertificate=True;",
  "sqlQuery": "SELECT [Key], [Value] FROM AppConfig",
  "azureAppConfig": [
    {
      "key": "AzureAppConfig:ConnectionString",
      "value": "Endpoint=https://jt-config-manager.azconfig.io;Id=...;Secret=..."
    }
  ],
  "vaultSecrets": [
    {
      "key": "AzureAppConfig:ConnectionString",
      "value": "..."
    },
    {
      "key": "DbKey1",
      "value": "Value from SQL"
    },
    {
      "key": "DbKey2",
      "value": "SQL Loaded Config"
    },
    {
      "key": "jt-vault-v1",
      "value": "topsecret$1"
    },
    {
      "key": "VaultSecret",
      "value": "myValue$1"
    },
    {
      "key": "jt-Key2",
      "value": "Key22"
    },
    {
      "key": "jt-Key1",
      "value": "Key11"
    }
  ]
}

```

🧪 Demo Project
See the live working example in the demo repo:
🔗 [JT.SmartConfigManager.Demo](https://github.com/JayantTripathy/JT-Nuget-Packages/tree/main/package-consume/SmartConfig-Demo/JT.SmartConfigManager.Demo)

Includes:

✅ SQL table with config values

✅ Azure Key Vault integration

✅ Azure AppConfig integration

✅ /config endpoint for viewing merged settings


✅ Benefits

Cleanly separate secrets and runtime config

Works with Azure Key Vault, App Config, and SQL

Easy to maintain for multi-env deployments

Strong typing and validation built-in

Avoid hardcoding secrets or fragile manual loading

📄 License

This project is licensed under the MIT License.