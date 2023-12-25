using System.Diagnostics.CodeAnalysis;

namespace LVK.Typed;

public interface ITypeNameRule
{
    int Priority { get; }

    string? TryGetNameOfType(Type type, ITypeHelper typeHelper, NameOfTypeOptions options);
}