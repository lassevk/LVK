namespace LVK.Typed;

public interface ITypeHelper
{
    string? TryGetNameOf(Type type, NameOfTypeOptions options = NameOfTypeOptions.Default);
}