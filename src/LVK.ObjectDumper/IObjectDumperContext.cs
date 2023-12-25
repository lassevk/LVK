namespace LVK.ObjectDumper;

public interface IObjectDumperContext
{
    IObjectDumperWriter Writer { get; }
    ObjectDumperOptions Options { get; }

    void Dump(string name, object? value, bool recursiveDump);
    void DumpProxy(string name, object value, string formattedValue, object? proxy, bool recursiveDump);
    void DumpProperties(object value, bool recursiveDump);
    void DumpFields(object value, bool recursiveDump);
}