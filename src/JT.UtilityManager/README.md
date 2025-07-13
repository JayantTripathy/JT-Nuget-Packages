# 🧰 JT.UtilityManager

A modular and extensible .NET utility library that simplifies integration of core application features like caching (in-memory & Redis), health checks, CORS, and future-ready tools such as file conversion, email sender, logging, and more.

---

🔗 [GitHub Repository of JT.UtilityManager](https://github.com/YourUsername/JT.UtilityManager) <br>
🔗 [GitHub Repository of Unit Test](https://github.com/YourUsername/JT.UtilityManager) <br>
🔗 [GitHub Repository of Sample Demo to learn how to use it](https://github.com/YourUsername/JT.UtilityManager)

## 🚀 Features


| Category           | Feature                          | Status |
|--------------------|----------------------------------|--------|
| 🧠 Caching          | In-Memory Cache                  | ✅ Ready |
| 🔁 Distributed      | Redis Cache (via StackExchange)  | ✅ Ready |
| ❤️ Monitoring       | Health Checks                    | ✅ Ready |
| 🌐 Networking       | CORS Policy Configuration        | 🛠 Planned |
| 📄 Conversion       | Word, PDF, CSV, JSON (Planned)   | 🛠 Planned |
| 📧 Communication    | Email Sending                    | 🛠 Planned |
| 🔐 Security         | Encryption/Decryption Utilities  | 🛠 Planned |
| 🔁 Resiliency       | Retry Policies                   | 🛠 Planned |
| 🪵 Logging          | Abstracted Logging Helpers       | 🛠 Planned |

---

## 📦 NuGet Installation

```bash
dotnet add package JT.UtilityManager
```

🛠 Setup

Register Everything

```csharp
builder.Services.AddUtilityManager(builder.Configuration);
```

Internally adds one by one according to the requirements:

```csharp
builder.Services.AddInMemoryCaching();

builder.Services.AddRedisCaching();

builder.Services.AddHealthChecks();

builder.Services.AddCors();

```

⚙️ Configuration (Example)
```json
{
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

🧪 Usage Examples

✅ In-Memory Cache

```csharp

public class ProductService
{
    private readonly IInMemoryCache _cache;

    public ProductService(IInMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        var key = $"product-{id}";
        return await _cache.GetOrSetAsync(key, () =>
        {
            // Fetch from DB or any external source
            return Task.FromResult(new Product { Id = id, Name = "Sample" });
        }, TimeSpan.FromMinutes(10));
    }
}

```

🔁 Redis Distributed Cache

```csharp
public class SessionService
{
    private readonly IDistributedCacheService _redis;

    public SessionService(IDistributedCacheService redis)
    {
        _redis = redis;
    }

    public async Task StoreSessionAsync(string userId, SessionData data)
    {
        await _redis.SetAsync($"session:{userId}", data, TimeSpan.FromHours(1));
    }

    public async Task<SessionData?> GetSessionAsync(string userId)
    {
        return await _redis.GetAsync<SessionData>($"session:{userId}");
    }
}
```

❤️ Health Check Endpoint

```csharp
app.MapHealthChecks("/health");
```

🌐 CORS Configuration

A default policy is already added:

```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
```

You can override this in your own Program.cs.


👩‍💻 Contributing

Want to add logging, email, file conversion or encryption support?
Great! Open an issue or submit a pull request.

Ideas to Contribute:

🔒 JWT Helper or Auth Middlewares

📤 SMTP + SendGrid email integration

📊 Telemetry wrapper for App Insights

📄 File parsing & format conversion (PDF/Excel)

📄 License

This library is licensed under the <a href="https://mit-license.org/"> MIT License</a>.


🔗 Related Package on Nuget

<a href="https://www.nuget.org/packages/JT.SmartConfigManager/">SmartConfigManager</a>


## Hi, I'm [Jayant Tripathy][<a href="https://jayanttripathy.com">website</a>] 👋 <img src="https://komarev.com/ghpvc/?username=JayantTripathy" alt="cprespider" align="center" />

- 🌱 I’m currently learning advanced concepts of AWS and Azure
- 👯 I’m looking to collaborate with other content creators on [<a href="https://www.youtube.com/@JayantT">YouTube</a>]
- 🥅 2024-2025 Goals: get 10k subscribers on YouTube
- ⚡ Fun fact: I love to watch cricket & listen to songs








                           🔧 Built and maintained with ❤️ by Jayant Tripathy


