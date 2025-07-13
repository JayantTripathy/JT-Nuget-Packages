# 🧰 JT-Nuget-Packages

Welcome to the **JT-Nuget-Packages** repository! This repository contains reusable .NET packages developed and maintained by **Jayant Tripathy**, aimed at accelerating enterprise-grade application development.

---

## 📦 Available Packages

### 1. 🔐 JT.SmartConfigManager

**JT.SmartConfigManager** is a unified configuration manager that allows you to dynamically load and merge application settings from:

- `appsettings.json`
- SQL database
- Azure Key Vault

#### 🌟 Features

- Strongly typed POCO mapping
- Dynamic configuration loading with precedence
- Azure Key Vault secret injection via interface
- SQL-based configuration provider
- Plug-and-play design via `SmartConfigManager<T>`

#### 🚀 Quick Start

```bash
dotnet add package JT.SmartConfigManager
```

# ⚙️ JT.UtilityManager

**JT.UtilityManager** is a modular, plug-and-play utility library for modern .NET applications. It provides essential, production-ready features to reduce boilerplate code and accelerate development — all under a unified and extensible framework.

---

## ✨ Key Features

| Category      | Features                                  |
|---------------|--------------------------------------------|
| ✅ Caching       | - In-Memory Caching<br>- Redis Caching       |
| ✅ Middleware    | - Global Exception Handling               |
| ✅ Health Checks | - ASP.NET Core HealthCheck integration    |
| ✅ CORS          | - Default CORS Setup                      |
| ✅ Rate Limiting | - Integrated with `AspNetCoreRateLimit`  |

---

## 🚀 Quick Installation

```bash
dotnet add package JT.UtilityManager
