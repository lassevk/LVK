namespace LVK.ObjectDumper.Rules;

public class StringObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes() => [typeof(string)];

    public bool SupportsType(Type type) => type == typeof(string);

    public int Priority => 10000;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        var s = value.ToString()!;
        int originalLength = s.Length;
        if (s.Length > context.Options.MaxStringDumpLength)
            s = s[.. context.Options.MaxStringDumpLength] + "...";

        int cr = s.IndexOfAny(['\n', '\r']);
        if (context.Options.StringsOnOneLine || cr < 0)
        {
            string formatted = s.Replace("\t", "\\t").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"") ?? "";
            context.Writer.WriteFormatted(name, value, $"\"{formatted}\"", out _);
            return;
        }

        context.Writer.WriteFormatted(name, value, $"Length: {originalLength}", out bool isFirstTime);
        if (!isFirstTime)
            return;

        context.Writer.BeginBlock();
        using var reader = new StringReader(s);
        while (reader.ReadLine() is { } line)
        {
            context.Writer.WriteLine(line);
        }

        context.Writer.EndBlock();
    }
}