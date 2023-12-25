namespace LVK.ObjectDumper;

public interface IObjectDumperRule
{
    Type[] GetKnownSupportedTypes();
    bool SupportsType(Type type);
    int Priority => 100_000;
    void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump);
}