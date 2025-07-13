using JT.SmartConfigManager.Core;
using JT.SmartConfigManager.Sources;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

#region Swagger Setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
#endregion

#region Load Base Configuration
var baseConfig = builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var earlySettings = new MyAppSettings();
baseConfig.Bind(earlySettings);
#endregion

#region SmartConfig Setup
var options = new SmartConfigOptions<MyAppSettings>();
options.Sources.Add(new JsonFileSource("appsettings.json"));
#endregion

#region Add SQL Config Source
if (!string.IsNullOrWhiteSpace(earlySettings.SqlConfig?.ConnectionString) &&
    !string.IsNullOrWhiteSpace(earlySettings.SqlConfig?.Query))
{
    options.Sources.Add(new SqlConfigSource(
        earlySettings.SqlConfig.ConnectionString,
        earlySettings.SqlConfig.Query));
    Console.WriteLine("✅ SQL source added");
}
else
{
    Console.WriteLine("⚠️ SQL config missing");
}
#endregion

#region Add Azure Key Vault Source
var vaultUri = baseConfig["KeyVault:VaultUri"];
if (!string.IsNullOrWhiteSpace(vaultUri))
{
    options.Sources.Add(new AzureKeyVaultSource<MyAppSettings>(vaultUri));
    Console.WriteLine("✅ Key Vault source added");
}
else
{
    Console.WriteLine("⚠️ Vault URI missing");
}
#endregion

#region Add Azure App Config Source
var azureAppConfigConn = earlySettings.AzureAppConfig?.ConnectionString;
if (!string.IsNullOrWhiteSpace(azureAppConfigConn))
{
    options.Sources.Add(new AzureAppConfigSource(azureAppConfigConn));
    Console.WriteLine("✅ AzureAppConfig source added");
}
else
{
    Console.WriteLine("⚠️ AzureAppConfig connection string not found.");
}
#endregion

#region Validations
options.Validate(s => !string.IsNullOrWhiteSpace(s?.AppSettings?.AppName), "AppName required");
options.Validate(s => !string.IsNullOrWhiteSpace(s?.AppSettings?.Environment), "Environment required");

#if !DEBUG
options.EnableAutoReload(TimeSpan.FromMinutes(5));
#endif
#endregion

#region Build Final Configuration
var configManager = new SmartConfigManager<MyAppSettings>(options);

var finalSettings = new MyAppSettings();
var mergedConfig = new ConfigurationBuilder()
    .AddInMemoryCollection(configManager.AllSettings!)
    .Build();
mergedConfig.Bind(finalSettings);

// ✅ Extract Vault Secrets (exclude known sections)
finalSettings.VaultSecretsDict = configManager.AllSettings?
    .Where(kv =>
        !kv.Key.StartsWith("AppSettings:") &&
        !kv.Key.StartsWith("SqlConfig:") &&
        !kv.Key.StartsWith("KeyVault:"))
    .ToDictionary(kv => kv.Key, kv => kv.Value!);

// ✅ Extract AzureAppConfig (only its keys)
finalSettings.AppConfigList = configManager.AllSettings?
    .Where(kv => kv.Key.StartsWith("AzureAppConfig:"))
    .Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value))
    .ToList();
#endregion

#region Print Final Settings (Console Debug)
Console.WriteLine("\n✅ Final Config:");
Console.WriteLine($"AppName: {finalSettings.AppSettings?.AppName}");
Console.WriteLine($"Environment: {finalSettings.AppSettings?.Environment}");
Console.WriteLine($"SQL Conn: {finalSettings.SqlConfig?.ConnectionString}");

if (finalSettings.VaultSecretsDict != null)
{
    Console.WriteLine("Vault Secrets:");
    foreach (var kv in finalSettings.VaultSecretsDict)
        Console.WriteLine($"  {kv.Key} = {kv.Value}");
}
else
{
    Console.WriteLine("❌ VaultSecretsDict is null");
}
#endregion

#region Register in DI
builder.Services.AddSingleton(finalSettings);
#endregion

var app = builder.Build();

#region Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // ✅ Renders /swagger/index.html
    app.MapOpenApi();   // Optional for minimal APIs
}
#endregion

app.UseHttpsRedirection();

#region Endpoints
app.MapGet("/config", (MyAppSettings settings) => new
{
    appName = settings.AppSettings?.AppName,
    environment = settings.AppSettings?.Environment,
    sqlConn = settings.SqlConfig?.ConnectionString,
    sqlQuery = settings.SqlConfig?.Query,
    AzureAppConfig = settings.AppConfigList ?? [],
    vaultSecrets = settings.VaultSecrets
});
#endregion

app.Run();
