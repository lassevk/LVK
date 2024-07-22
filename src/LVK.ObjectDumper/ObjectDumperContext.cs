using System.Reflection;

using LVK.ObjectDumper.Rules;

namespace LVK.ObjectDumper;

internal class ObjectDumperContext : IObjectDumperContext
{
    private readonly IObjectDumperRule _fallbackRule = new FallbackObjectDumperRule();
    private readonly List<IObjectDumperRule> _rules;
    private readonly Dictionary<Type, IObjectDumperRule> _typeToRule = new();

    private int _recursionLevel;

    public ObjectDumperContext(List<IObjectDumperRule> rules, ObjectDumperOptions options, TextWriter target)
    {
        foreach (IObjectDumperRule rule in rules.OrderByDescending(rule => rule.Priority))
        {
            foreach (Type supportedType in rule.GetKnownSupportedTypes())
                _typeToRule[supportedType] = rule;
        }

        _rules = rules.OrderBy(rule => rule.Priority).ToList();
        Options = options;
        Writer = new ObjectDumperWriter(target, options);
    }

    public IObjectDumperWriter Writer { get; }
    public ObjectDumperOptions Options { get; }

    public void Dump(string name, object? value, bool recursiveDump)
    {
        _recursionLevel++;
        try
        {
            if (value == null)
            {
                Writer.WriteLine($"{name} = <null>");
                return;
            }

            IObjectDumperRule rule = GetRuleForType(value.GetType());
            rule.Dump(this, name, value, recursiveDump && _recursionLevel < Options.MaxRecursionLevel);
        }
        finally
        {
            _recursionLevel--;
        }
    }

    public void DumpProxy(string name, object value, string formattedValue, object? proxy, bool recursiveDump)
    {
        _recursionLevel++;
        try
        {
            Writer.WriteFormatted(name, value, formattedValue, out bool isFirstTime);
            if (!isFirstTime || proxy is null || !recursiveDump)
                return;

            Writer.BeginBlock();
            DumpProperties(proxy, recursiveDump && _recursionLevel < Options.MaxRecursionLevel);
            Writer.EndBlock();
        }
        finally
        {
            _recursionLevel--;
        }
    }

    public void DumpProperties(object value, bool recursiveDump)
    {
        BindingFlags bindingFlags = Options.IncludePrivateMembers ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic : BindingFlags.Instance | BindingFlags.Public;

        PropertyInfo[] properties = value.GetType().GetProperties(bindingFlags);
        if (!properties.Any())
            return;

        Writer.BeginBlock("properties");

        foreach (PropertyInfo property in properties)
        {
            try
            {
                object? propertyValue = property.GetValue(value);
                Dump(property.Name, propertyValue, recursiveDump && !Equals(propertyValue, value));
            }
            catch (TargetParameterCountException ex)
            {
                Dump(property.Name, ex, false);
            }
            catch (TargetInvocationException ex)
            {
                Dump(property.Name, ex, false);
            }
            catch (ArgumentException ex)
            {
                Dump(property.Name, ex, false);
            }
        }

        Writer.EndBlock();
    }

    public void DumpFields(object value, bool recursiveDump)
    {
        BindingFlags bindingFlags = Options.IncludePrivateMembers ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic : BindingFlags.Instance | BindingFlags.Public;

        FieldInfo[] fields = value.GetType().GetFields(bindingFlags);
        if (!fields.Any())
            return;

        Writer.BeginBlock("fields");

        foreach (FieldInfo field in fields)
        {
            try
            {
                object? fieldValue = field.GetValue(value);
                Dump(field.Name, fieldValue, recursiveDump && !Equals(fieldValue, value));
            }
            catch (TargetInvocationException ex)
            {
                Dump(field.Name, ex, false);
            }
            catch (ArgumentException ex)
            {
                Dump(field.Name, ex, false);
            }
        }

        Writer.EndBlock();
    }

    private IObjectDumperRule GetRuleForType(Type type)
    {
        if (_typeToRule.TryGetValue(type, out IObjectDumperRule? existingRule))
            return existingRule;

        foreach (IObjectDumperRule rule in _rules)
        {
            if (rule.SupportsType(type))
            {
                _typeToRule[type] = rule;
                return rule;
            }
        }

        _typeToRule[type] = _fallbackRule;
        return _fallbackRule;
    }
}