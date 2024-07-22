namespace LVK.ObjectDumper.Rules;

public class EnumObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes() => [];

    public bool SupportsType(Type type) => type.IsEnum;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        Type enumType = value.GetType();
        string formatted = Enum.Format(enumType, value, "G");
        Type underlyingType = Enum.GetUnderlyingType(enumType);

        var formattable = (IFormattable)Convert.ChangeType(value, underlyingType);
        var numericValueFormatted = $"{formattable:G} = 0x{formattable:X} = 0b{formattable:B}";
        context.Writer.WriteFormatted(name, value, $"{formatted} ({numericValueFormatted})", out _);
    }
}