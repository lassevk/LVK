namespace LVK.Typed;

internal class TypeHelper : ITypeHelper
{
    private readonly List<ITypeNameRule> _typeNameRules;

    public TypeHelper(IEnumerable<ITypeNameRule> nameOfRules)
    {
        ArgumentNullException.ThrowIfNull(nameOfRules);

        _typeNameRules = nameOfRules.OrderBy(r => r.Priority).ToList();
    }

    string? ITypeHelper.TryGetNameOf(Type type, NameOfTypeOptions options)
    {
        ArgumentNullException.ThrowIfNull(type);

        return _typeNameRules.Select(r => r.TryGetNameOfType(type, this, options)).FirstOrDefault(n => !(n is null));
    }
}