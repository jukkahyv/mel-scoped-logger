using Microsoft.Extensions.Logging;

namespace Rapal.ScopedLogger;

/// <summary>
/// Extensions for <see cref="ILogger"/>.
/// </summary>
public static class LoggerExtensions
{
    public static IScopedLogger Chain(this ILogger logger) =>
        logger as IScopedLogger ?? new ScopedLogger(logger, new Dictionary<string, object>());
    
    public static IScopedLogger ForContext(this ILogger logger, string key, object value) =>
        logger.Chain().ForContext(key, value);
}