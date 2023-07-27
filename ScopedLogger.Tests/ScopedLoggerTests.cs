using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Rapal.ScopedLogger.Tests;

public class ScopedLoggerTests
{
    private readonly MyLogProvider _logProvider = new();
    
    [Fact]
    public void Test()
    {
        var logger = GetLogger();
        
        ILogger loggerWithProperty = logger.ForContext("MyProperty", "MyValue").ForContext("MyProperty2", "MyValue2");
        loggerWithProperty.LogInformation("Hello World");

        var logEvent = _logProvider.LogMessages.Should().ContainSingle().Subject;
        logEvent.Message.Should().Be("Hello World");
        var scopeState = logEvent.ScopeState.Should().BeAssignableTo<IReadOnlyDictionary<string, object>>().Subject;
        scopeState.Should().ContainKey("MyProperty").WhoseValue.Should().Be("MyValue");
        scopeState.Should().ContainKey("MyProperty2").WhoseValue.Should().Be("MyValue2");
    }

    [Fact]
    public void DuplicateKeys()
    {
        var logger = GetLogger();
        
        ILogger loggerWithProperty = logger.ForContext("MyProperty", "MyValue").ForContext("MyProperty", "MyValue2");
        loggerWithProperty.LogInformation("Hello World");

        var logEvent = _logProvider.LogMessages.Should().ContainSingle().Subject;
        logEvent.Message.Should().Be("Hello World");
        var scopeState = logEvent.ScopeState.Should().BeAssignableTo<IReadOnlyDictionary<string, object>>()
            .Which.Should().ContainKey("MyProperty").WhoseValue.Should().Be("MyValue2");
    }

    private ILogger GetLogger() => new LoggerFactory(new[] { _logProvider })
        .CreateLogger<ScopedLoggerTests>();
}

