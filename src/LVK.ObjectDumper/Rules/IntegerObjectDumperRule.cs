using System.Globalization;

namespace LVK.ObjectDumper.Rules;

public class IntegerObjectDumperRule : IObjectDumperRule
{
    private readonly HashSet<Type> _supportedTypes = [
        typeof(long), typeof(ulong),
        typeof(int), typeof(uint),
        typeof(short), typeof(ushort),
        typeof(sbyte), typeof(byte),
        typeof(Int128), typeof(UInt128),
    ];

    public Type[] GetKnownSupportedTypes() => _supportedTypes.ToArray();

    public bool SupportsType(Type type) => _supportedTypes.Contains(type);

    public int Priority => 10000;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        var formatted = ((IFormattable)value).ToString("G", context.Options.FormattingCulture);
        context.Writer.WriteFormatted(name, value, formatted, out _);
    }
}