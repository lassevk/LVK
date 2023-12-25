using System.Reflection;
using System.Security.Principal;

namespace LVK.ObjectDumper.Rules;

public class NonRecursiveTypesObjectDumperRule : IObjectDumperRule
{
    private readonly HashSet<Type> _supportedTypes = [
        typeof(SecurityIdentifier),
        typeof(RuntimeArgumentHandle),
        typeof(RuntimeFieldHandle),
        typeof(RuntimeMethodHandle),
        typeof(RuntimeTypeHandle),
        typeof(Assembly),
        typeof(AppDomain),
    ];

    public Type[] GetKnownSupportedTypes() => _supportedTypes.ToArray();

    public bool SupportsType(Type type)
    {
        if (_supportedTypes.Contains(type))
            return true;

        return false;
    }

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        context.Writer.WriteFormatted(name, value, value.ToString() ?? "", out _);
    }
}