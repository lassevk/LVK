namespace LVK.Typed.Rules;

internal class NormalTypeNameRule : ITypeNameRule
{
    public int Priority => int.MaxValue;

    public string? TryGetNameOfType(Type type, ITypeHelper typeHelper, NameOfTypeOptions options)
        => type.IsGenericType ? null : (options & NameOfTypeOptions.IncludeNamespaces) != 0 ? type.FullName : type.Name;
}