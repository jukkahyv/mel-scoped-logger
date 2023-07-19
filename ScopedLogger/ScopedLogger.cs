using Microsoft.Extensions.Logging;

namespace ScopedLogger;

/// <summary>
/// Extends MEL <see cref="ILogger"/> with context/scope chaining features similar to Serilog.
/// The attempt is to have a logger interface with compatibility of MEL and features of Serilog.
/// </summary>
public interface IScopedLogger : ILogger
{
    /// <summary>
    /// Creates scope with state from current logger's context.
    /// </summary>
    IDisposable? BeginScope();
    
    /// <summary>
    /// Creates a new logger with a property added to log context.
    /// </summary>
    /// <remarks>
    /// The syntax resembles Serilog's ForContext method.
    /// See https://github.com/serilog/serilog/wiki/Writing-Log-Events#correlation
    /// </remarks>
    IScopedLogger ForContext(string key, object value);

    IScopedLogger ForContext(IEnumerable<KeyValuePair<string, object>> context) 
        => context.Aggregate(this, (logger, pair) => logger.ForContext(pair.Key, pair.Value));
} 

internal class ScopedLogger : IScopedLogger
{
    public ScopedLogger(ILogger parent, Dictionary<string, object> state)
    {
        _parent = parent;
        _state = state;
    }

    private readonly ILogger _parent;
    private readonly Dictionary<string, object> _state;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        using var scope = BeginScope();
        _parent.Log(logLevel, eventId, state, exception, formatter);
    }

    public bool IsEnabled(LogLevel logLevel) => _parent.IsEnabled(logLevel);

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _parent.BeginScope(state);

    public IDisposable? BeginScope() => BeginScope(_state);

    public IScopedLogger ForContext(string key, object value)
    {
        return new ScopedLogger(_parent,
            new Dictionary<string, object>(_state.Append(KeyValuePair.Create(key, value))));
    }
    
}