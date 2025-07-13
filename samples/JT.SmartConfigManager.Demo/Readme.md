
 
 ```json
{
  "appName": "Demo API",
  "environment": "Development",
  "sqlConn": "Server=localhost\\SQLEXPRESS01;Database=testDB;Trusted_Connection=True;TrustServerCertificate=True;",
  "sqlQuery": "SELECT [Key], [Value] FROM AppConfig",
  "azureAppConfig": [
    {
      "key": "AzureAppConfig:ConnectionString",
      "value": "Endpoint=https://jt-config-manager.azconfig.io;Id=iqly;Secret=********"
    }
  ],
  "vaultSecrets": [
    {
      "key": "AzureAppConfig:ConnectionString",
      "value": "Endpoint=https://jt-config-manager.azconfig.io;Id=iqly;Secret=********"
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
