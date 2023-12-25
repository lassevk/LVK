using System.Globalization;

namespace LVK.ObjectDumper.Rules;

public class NumericObjectDumperRule : IObjectDumperRule
{
    private readonly HashSet<Type> _supportedTypes = [
        typeof(decimal),
        typeof(float),
        typeof(double),
    ];

    public Type[] GetKnownSupportedTypes() => _supportedTypes.ToArray();

    public bool SupportsType(Type type) => _supportedTypes.Contains(type);

    public int Priority => 10000;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        var formatted = ((IFormattable)value).ToString("R", context.Options.FormattingCulture);

        context.Writer.WriteFormatted(name, value, formatted, out _);
    }
}