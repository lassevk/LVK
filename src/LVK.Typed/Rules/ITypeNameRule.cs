namespace LVK.Typed.Rules;

public interface ITypeNameRule
{
    int Priority { get; }

    string? TryGetNameOfType(Type type, ITypeHelper typeHelper, NameOfTypeOptions options);
}