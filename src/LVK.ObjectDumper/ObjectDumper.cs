using LVK.ObjectDumper.Rules;
using LVK.Typed;

namespace LVK.ObjectDumper;

public class ObjectDumper : IObjectDumper
{
    private readonly List<IObjectDumperRule> _rules = new();
    private readonly object _lock = new();

    public static IObjectDumper Instance { get; } = CreateDefaultObjectDumper();

    private static IObjectDumper CreateDefaultObjectDumper()
    {
        var result = new ObjectDumper();

        result.AddRule(new StringObjectDumperRule());
        result.AddRule(new IntegerObjectDumperRule());
        result.AddRule(new BooleanObjectDumperRule());
        result.AddRule(new NumericObjectDumperRule());
        result.AddRule(new IntPtrObjectDumperRule());
        result.AddRule(new PointerObjectDumperRule());
        result.AddRule(new NonRecursiveTypesObjectDumperRule());
        result.AddRule(new TypeObjectDumperRule());
        result.AddRule(new AssemblyObjectDumperRule());
        result.AddRule(new GuidObjectDumperRule());
        result.AddRule(new EnumerableObjectDumperRule());
        result.AddRule(new DateOrTimeTypesObjectDumperRule());
        result.AddRule(new EnumerableObjectDumperRule());
        result.AddRule(new EnumObjectDumperRule());
        result.AddRule(new ByteArrayObjectDumperRule());
        result.AddRule(new ExceptionObjectDumperRule());

        return result;
    }

    public void AddRule(IObjectDumperRule rule)
    {
        if (rule == null)
            throw new ArgumentNullException(nameof(rule));

        lock (_rules)
        {
            _rules.Add(rule);
        }
    }

    public void Dump(string name, object value, TextWriter target, ObjectDumperOptions? options = null)
    {
        List<IObjectDumperRule> rules;
        lock (_rules)
        {
            rules = _rules.ToList();
        }

        new ObjectDumperContext(rules, options ?? new(), target).Dump(name, value, true);
    }
}