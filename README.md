# Microsoft.Extensions.Logging scoped logger

Extends MEL ILogger with context/scope chaining features similar to Serilog.

## Installation

```powershell 
PM> Install-Package Rapal.ScopedLogger
```

## Usage

```csharp
var logger = _log.ForContext("MyProp", "MyValue");
logger.LogInformation("Hello world!");
```