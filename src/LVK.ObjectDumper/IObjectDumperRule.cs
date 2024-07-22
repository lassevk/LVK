namespace LVK.ObjectDumper;

public interface IObjectDumperRule
{
    int Priority => 100_000;
    Type[] GetKnownSupportedTypes();
    bool SupportsType(Type type);
    void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump);
}