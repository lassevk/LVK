namespace LVK.ObjectDumper;

public interface IObjectDumper
{
    void Dump(string name, object value, TextWriter target, ObjectDumperOptions? options = null);
}