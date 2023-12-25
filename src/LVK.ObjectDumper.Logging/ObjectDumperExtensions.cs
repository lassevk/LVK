using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;

namespace LVK.ObjectDumper.Logging;

public static class ObjectDumperExtensions
{
    public static void Dump(this IObjectDumper dumper, string name, object value, ILogger logger, ObjectDumperOptions? options = null) => Dump(dumper, LogLevel.Debug, name, value, logger, options);
    public static void Dump(this IObjectDumper dumper, LogLevel logLevel, string name, object value, ILogger logger, ObjectDumperOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            name = "<unnamed>";

        string output = dumper.Dump(name, value, options);
        logger.Log(logLevel, "{ObjectDump}", output);
    }

    public static void Dump(this IObjectDumper dumper, object value, ILogger logger, ObjectDumperOptions? options = null, [CallerArgumentExpression(nameof(value))] string? name = null)
        => Dump(dumper, LogLevel.Debug, value, logger, options, name);
    public static void Dump(this IObjectDumper dumper, LogLevel logLevel, object value, ILogger logger, ObjectDumperOptions? options = null, [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            name = "<unnamed>";

        Dump(dumper, logLevel, name, value, logger, options);
    }
}