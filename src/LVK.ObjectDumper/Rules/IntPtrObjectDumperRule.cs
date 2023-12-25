using System.Globalization;

namespace LVK.ObjectDumper.Rules;

public class IntPtrObjectDumperRule : IObjectDumperRule
{
    private readonly HashSet<Type> _supportedTypes = [
        typeof(nint),
        typeof(nuint),
    ];

    public Type[] GetKnownSupportedTypes() => _supportedTypes.ToArray();

    public bool SupportsType(Type type) => _supportedTypes.Contains(type);

    public int Priority => 10000;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        var formatted = $"0x{((IFormattable)value).ToString("x16", context.Options.FormattingCulture)}";

        context.Writer.WriteFormatted(name, value, formatted, out _);
    }
}