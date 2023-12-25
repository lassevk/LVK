using System.Globalization;

namespace LVK.ObjectDumper.Rules;

public class DateOrTimeTypesObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes() => [typeof(DateTime), typeof(DateTimeOffset), typeof(DateOnly), typeof(TimeOnly), typeof(TimeSpan)];

    public bool SupportsType(Type type) => false;

    public int Priority => 10000;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        string formatted = value switch
        {
            DateTime dt        => dt.ToString("G", context.Options.FormattingCulture),
            DateTimeOffset dto => dto.ToString("G", context.Options.FormattingCulture),
            DateOnly d         => d.ToString("G", context.Options.FormattingCulture),
            TimeOnly t         => t.ToString("G", context.Options.FormattingCulture),
            TimeSpan ts        => ts.ToString("G", context.Options.FormattingCulture),
            _                  => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

        context.Writer.WriteFormatted(name, value, formatted, out _);
    }
}