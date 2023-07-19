using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace ScopedLogger.Tests;

public class ScopedLoggerTests
{
    [Fact]
    public void Test()
    {
        var logProvider = new MyLogProvider();
        var logger = new LoggerFactory(new[] { logProvider })
            .CreateLogger<ScopedLoggerTests>();
        
        var loggerWithProperty = logger.ForContext("MyProperty", "MyValue");
        loggerWithProperty.LogInformation("Hello World");

        var logEvent = logProvider.LogMessages.Should().ContainSingle().Subject;
        logEvent.Message.Should().Be("Hello World");
        logEvent.ScopeState.Should().BeAssignableTo<IReadOnlyDictionary<string, object>>()
            .Which.Should().ContainKey("MyProperty").WhoseValue.Should().Be("MyValue");
    }
}

