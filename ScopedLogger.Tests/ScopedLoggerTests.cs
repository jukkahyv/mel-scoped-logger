using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace ScopedLogger.Tests;

public class ScopedLoggerTests
{
    private readonly MyLogProvider _logProvider = new();
    
    [Fact]
    public void Test()
    {
        var logger = GetLogger();
        
        ILogger loggerWithProperty = logger.ForContext("MyProperty", "MyValue");
        loggerWithProperty.LogInformation("Hello World");

        var logEvent = _logProvider.LogMessages.Should().ContainSingle().Subject;
        logEvent.Message.Should().Be("Hello World");
        logEvent.ScopeState.Should().BeAssignableTo<IReadOnlyDictionary<string, object>>()
            .Which.Should().ContainKey("MyProperty").WhoseValue.Should().Be("MyValue");
    }

    private ILogger GetLogger() => new LoggerFactory(new[] { _logProvider })
        .CreateLogger<ScopedLoggerTests>();
}

