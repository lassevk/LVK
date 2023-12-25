using LVK.Typed;

namespace LVK.ObjectDumper;

internal class ObjectDumper : IObjectDumper
{
    private readonly ITypeHelper _typeHelper;
    private readonly List<IObjectDumperRule> _rules;

    public ObjectDumper(ITypeHelper typeHelper, IEnumerable<IObjectDumperRule> rules)
    {
        _typeHelper = typeHelper ?? throw new ArgumentNullException(nameof(typeHelper));
        _rules = (rules ?? throw new ArgumentNullException(nameof(rules))).ToList();
    }

    public void Dump(string name, object value, TextWriter target, ObjectDumperOptions? options = null)
    {
        new ObjectDumperContext(_rules, _typeHelper, options ?? new(), target).Dump(name, value, true);
    }
}