namespace LVK.ObjectDumper.Rules;

public class GuidObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes() => [typeof(Guid)];

    public bool SupportsType(Type type) => type == typeof(Guid);

    public int Priority => 10000;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump) => context.Writer.WriteFormatted(name, value, $"\"{(Guid)value}\"", out _);
}