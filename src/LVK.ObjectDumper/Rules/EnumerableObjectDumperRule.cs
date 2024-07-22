using System.Collections;

namespace LVK.ObjectDumper.Rules;

public class EnumerableObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes() => [];

    public bool SupportsType(Type type) => typeof(IEnumerable).IsAssignableFrom(type) || typeof(IDictionary).IsAssignableFrom(type);

    public int Priority => int.MaxValue;

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        if (value is IDictionary dict)
            DumpDictionary(context, name, dict, recursiveDump);
        else if (value is IEnumerable en)
            DumpEnumerable(context, name, en, recursiveDump);
    }

    private void DumpEnumerable(IObjectDumperContext context, string name, IEnumerable enumerable, bool recursiveDump)
    {
        string formatted = enumerable is ICollection c ? $"Count: {c.Count}" : "<unknown count>";
        context.Writer.WriteFormatted(name, enumerable, formatted, out bool isFirstTime);
        if (!isFirstTime)
            return;

        IEnumerator enumerator = enumerable.GetEnumerator();
        try
        {
            if (!enumerator.MoveNext())
                return;

            context.Writer.BeginBlock();
            context.Writer.BeginBlock("items");

            var index = 0;
            var more = true;
            while (more)
            {
                context.Dump($"#{index}", enumerator.Current, recursiveDump);
                index++;
                more = enumerator.MoveNext();
            }

            context.Writer.EndBlock();
            context.Writer.EndBlock();
        }
        finally
        {
            (enumerator as IDisposable)?.Dispose();
        }
    }

    private void DumpDictionary(IObjectDumperContext context, string name, IDictionary dictionary, bool recursiveDump)
    {
        var formatted = $"Count: {dictionary.Count}";
        context.Writer.WriteFormatted(name, dictionary, formatted, out bool isFirstTime);
        if (!isFirstTime || dictionary.Count == 0)
            return;

        context.Writer.BeginBlock();
        context.Writer.BeginBlock("items");

        var index = 0;
        foreach (DictionaryEntry kvp in dictionary)
        {
            context.Writer.BeginBlock($"item #{index}");
            context.Dump("Key", kvp.Key, recursiveDump);
            context.Dump("Value", kvp.Value, recursiveDump);
            context.Writer.EndBlock();
            index++;
        }

        context.Writer.EndBlock();
        context.Writer.EndBlock();
    }
}