using System.Reflection;

namespace LVK.ObjectDumper.Rules;

public class AssemblyObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes()  => [];

    public bool SupportsType(Type type) => type.FullName == "System.Reflection.RuntimeAssembly";

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        var assembly = (Assembly)value;
        context.DumpProxy(name, value, $"Assembly: {assembly.Location}", new
        {
            assembly.Location,
            assembly.FullName,
        }, recursiveDump);
    }
}