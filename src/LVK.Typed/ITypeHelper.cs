using LVK.Typed.Rules;

namespace LVK.Typed;

public interface ITypeHelper
{
    void AddRule(ITypeNameRule rule);
    string? TryGetNameOf(Type type, NameOfTypeOptions options = NameOfTypeOptions.Default);
}