namespace LVK.ObjectDumper;

public interface IObjectDumperWriter
{
    void WriteLine(string line);
    void Indent();
    void Outdent();
    string FormatType(Type type);
    void WriteFormatted(string name, object value, string formatted, out bool isFirstTime);
    void BeginBlock(string name = "");
    void EndBlock();
}