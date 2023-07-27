using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Rapal.ScopedLogger.Tests;

public class ScopedLoggerTests
{
    private readonly MyLogProvider _logProvider = new();
    private readonly ILogger _logger;

    public ScopedLoggerTests()
    {
        _logger = new LoggerFactory(new[] { _logProvider })
            .CreateLogger<ScopedLoggerTests>();
    }
    
    [Fact]
    public void Test()
    {
        ILogger loggerWithProperty = _logger.ForContext("MyProperty", "MyValue").ForContext("MyProperty2", "MyValue2");
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
        ILogger loggerWithProperty = _logger.ForContext("MyProperty", "MyValue").ForContext("MyProperty", "MyValue2");
        loggerWithProperty.LogInformation("Hello World");

        var logEvent = _logProvider.LogMessages.Should().ContainSingle().Subject;
        logEvent.Message.Should().Be("Hello World");
        logEvent.ScopeState.Should().BeAssignableTo<IReadOnlyDictionary<string, object>>()
            .Which.Should().ContainKey("MyProperty").WhoseValue.Should().Be("MyValue2");
    }
    
}

