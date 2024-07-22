namespace LVK.ObjectDumper.Rules;

public class TypeObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes() => [];

    public bool SupportsType(Type type) => type.FullName == "System.RuntimeType";

    public int Priority => 10000;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        var t = (Type)value;
        context.DumpProxy(name, value, $"Type: {context.Writer.FormatType(t)}", new
        {
            t.Namespace, t.Assembly,
        }, recursiveDump);
    }
}