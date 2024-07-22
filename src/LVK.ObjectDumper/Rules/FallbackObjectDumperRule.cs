using System.Reflection;

namespace LVK.ObjectDumper.Rules;

internal class FallbackObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes() => throw new NotSupportedException();

    public bool SupportsType(Type type) => throw new NotSupportedException();

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        string formatted = value.ToString()?.Replace("\t", "\\t").Replace("\n", "\\n").Replace("\r", "\\r") ?? "";
        if (formatted == value.GetType().FullName)
            formatted = "";

        if (formatted.Length > 80)
        {
            formatted = formatted[..80] + "...";
        }

        context.Writer.WriteFormatted(name, value, formatted, out bool isFirstTime);
        if (!isFirstTime)
            return;

        if (!recursiveDump)
            return;

        PropertyInfo[] properties = value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        FieldInfo[] fields = value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (!properties.Any() && !fields.Any())
            return;

        context.Writer.BeginBlock();
        context.DumpProperties(value, recursiveDump);
        if (context.Options.IncludeFields)
            context.DumpFields(value, recursiveDump);

        context.Writer.EndBlock();
    }
}