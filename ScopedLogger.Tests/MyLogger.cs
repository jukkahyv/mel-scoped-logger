using Microsoft.Extensions.Logging;

namespace ScopedLogger.Tests;

internal record MyLogMessage(string Message, object? State)
{
    
}

internal class MyLogProvider : ILoggerProvider
{
    public List<MyLogMessage> LogMessages = new(); 
    
    public void Dispose()
    {
        
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new MyLogger(this);
    }
}

internal class MyLogger : ILogger
{
    public MyLogger(MyLogProvider logProvider)
    {
        _logProvider = logProvider;
    }
    
    private readonly MyLogProvider _logProvider;
    
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var formatted = formatter(state, exception);
        _logProvider.LogMessages.Add(new MyLogMessage(formatted, state));
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return new MyScope();
    }
}

internal class MyScope : IDisposable
{
    public void Dispose()
    {
        
    }
}