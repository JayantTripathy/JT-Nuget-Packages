using JT.SmartConfigManager.Core;
using JT.SmartConfigManager.Demo;
using JT.SmartConfigManager.Sources;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Step 1: Create SmartConfigManager instance with config sources
var options = new SmartConfigOptions<MyAppSettings>();
options.Sources.Add(new JsonFileSource("appsettings.json"));
//options.Sources.Add(new AzureAppConfigSource("AzureAppConfig:ConnectionString"));
//options.Sources.Add(new AzureKeyVaultSource("KeyVault:VaultUri"));
options.Sources.Add(new SqlConfigSource("SqlConfig:ConnectionString", "SqlConfig:Query"));

options.Validate(settings => !string.IsNullOrEmpty(settings?.AppSettings?.AppName), "AppName must be set!");
options.Validate(settings => !string.IsNullOrEmpty(settings?.AppSettings?.Environment), "Environment must be set!");

#if !DEBUG
options.EnableAutoReload(TimeSpan.FromMinutes(5));
#endif

var configManager = new SmartConfigManager<MyAppSettings>(options);



// Merge with default config
builder.Services.AddSingleton(configManager.Current);
var config = builder.Configuration;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var sqlConfig = new SqlConfigSettings();
config.GetSection("SqlConfig").Bind(sqlConfig);

// Health check endpoint to test SQL connection
app.MapGet("/test-sql", async () =>
{
    try
    {
        await using var conn = new SqlConnection(sqlConfig.ConnectionString);
        await conn.OpenAsync();

        var cmd = new SqlCommand(sqlConfig.Query, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        var results = new Dictionary<string, string>();
        while (await reader.ReadAsync())
        {
            string key = reader.GetString(0);
            string value = reader.GetString(1);
            results[key] = value;
        }

        return Results.Ok(new
        {
            message = "✅ SQL query success.",
            values = results
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"❌ SQL failed: {ex.Message}");
    }
});

// Minimal API endpoint to show config
app.MapGet("/config", (IConfiguration config) => new
{
    AppName = config["AppSettings:AppName"],
    Environment = config["AppSettings:Environment"],
    SqlValue = config["DbKey1"], // from SQL
    Secret = config["AppSecret"] // from Key Vault
});

app.Run();
