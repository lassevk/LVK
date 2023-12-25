using System.Runtime.CompilerServices;

namespace LVK.ObjectDumper;

public static class ObjectDumperExtensions
{
    public static void Dump(this IObjectDumper dumper, object value, TextWriter target, ObjectDumperOptions? options = null, [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            name = "<unnamed>";
        dumper.Dump(name, value, target, options);
    }

    public static string Dump(this IObjectDumper dumper, string name, object value, ObjectDumperOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            name = "<unnamed>";

        var target = new StringWriter();
        dumper.Dump(name, value, target, options);
        return target.ToString();
    }

    public static string Dump(this IObjectDumper dumper, object value, ObjectDumperOptions? options = null, [CallerArgumentExpression(nameof(value))] string? name = null)
        => Dump(dumper, name ?? "", value, options);
}