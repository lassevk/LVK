namespace LVK.ObjectDumper;

public interface IObjectDumper
{
    void AddRule(IObjectDumperRule rule);
    void Dump(string name, object value, TextWriter target, ObjectDumperOptions? options = null);
}