# Microsoft.Extensions.Logging scoped logger

Extends [MEL ILogger](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging) with context/scope chaining 
features similar to [Serilog](https://github.com/serilog/serilog/wiki/Writing-Log-Events#correlation).

## Installation

```powershell 
PM> Install-Package Rapal.ScopedLogger
```

## Usage

```csharp
ILogger logger = _log.ForContext("MyProp", "MyValue");
logger.LogInformation("Hello world!");
```