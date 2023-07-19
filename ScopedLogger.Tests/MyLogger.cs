using Microsoft.Extensions.Logging;

namespace ScopedLogger.Tests;

internal record MyLogMessage(string Message, object? ScopeState);

internal class MyLogProvider : ILoggerProvider
{
    public readonly List<MyLogMessage> LogMessages = new(); 
    
    public void Dispose()
    {
        
    }

    public ILogger CreateLogger(string categoryName) => new MyLogger(this);
}

internal class MyLogger : ILogger
{
    public MyLogger(MyLogProvider logProvider)
    {
        _logProvider = logProvider;
    }
    
    private readonly MyLogProvider _logProvider;
    public object? ScopeState { get; set; }
    
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var formatted = formatter(state, exception);
        _logProvider.LogMessages.Add(new MyLogMessage(formatted, ScopeState));
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        ScopeState = state;
        return new MyScope(this);
    }
}

internal class MyScope : IDisposable
{
    public MyScope(MyLogger logger)
    {
        _logger = logger;
    }
    
    private readonly MyLogger _logger;
    
    public void Dispose()
    {
        _logger.ScopeState = null;
    }
}