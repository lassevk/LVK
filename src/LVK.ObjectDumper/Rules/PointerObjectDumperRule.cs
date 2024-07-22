namespace LVK.ObjectDumper.Rules;

public class PointerObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes() => [];

    public bool SupportsType(Type type) => type.IsPointer || type.IsFunctionPointer || type.IsUnmanagedFunctionPointer;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        throw new NotSupportedException($"Type {context.Writer.FormatType(value.GetType())} is a pointer type, report this as an issue with a reproducible example and support will be added");
    }
}