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

        logProvider.LogMessages.Should().ContainSingle();
    }
}

