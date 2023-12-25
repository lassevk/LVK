namespace LVK.ObjectDumper.Rules;

internal class BooleanObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes() => [typeof(bool)];

    public bool SupportsType(Type type) => type == typeof(bool);

    public int Priority => 10000;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump) => context.Writer.WriteFormatted(name, value, (bool)value ? "true" : "false", out _);
}