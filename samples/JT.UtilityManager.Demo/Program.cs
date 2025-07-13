using JT.UtilityManager.Demo.Services;
using JT.UtilityManager.Extensions;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Load appsettings
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

// ✅ Register all utility features (InMemory, Redis, CORS, HealthChecks, etc.)
builder.Services.AddUtilityManager(builder.Configuration);

// ✅ Register app-level services
builder.Services.AddSingleton<ProductService>();

// ✅ Register controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Dev tools (Swagger, OpenAPI)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwagger();
}

// ✅ Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
