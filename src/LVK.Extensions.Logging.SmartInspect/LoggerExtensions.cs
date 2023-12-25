using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;

namespace LVK.Extensions.Logging.SmartInspect;

public static class LoggerExtensions
{
    public static IDisposable? BeginMethodScope(this ILogger logger, object instance, [CallerMemberName] string? memberName = null) => BeginMethodScope(logger, instance.GetType(), memberName);

    public static IDisposable? BeginMethodScope<T>(this ILogger logger, [CallerMemberName] string? memberName = null) => BeginMethodScope(logger, typeof(T), memberName);

    public static IDisposable? BeginMethodScope(this ILogger logger, Type instanceType, [CallerMemberName] string? memberName = null) => logger.BeginScope($"{instanceType.FullName}.{memberName}");

    public static IDisposable? BeginLocalScope(
        this ILogger logger, string? name = null, [CallerFilePath] string? callerFilePath = null, [CallerMemberName] string? callerMemberName = null, [CallerLineNumber] int? callerLineNumber = 0)
    {
        name ??= $"{callerMemberName}#{callerLineNumber}";
        var scopeName = $"{name} [{callerFilePath}#{callerLineNumber}]";
        return logger.BeginScope(scopeName);
    }
}