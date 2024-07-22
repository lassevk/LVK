using LVK.Core;
using LVK.Typed.Rules;

namespace LVK.Typed;

public class TypeHelper : ITypeHelper
{
    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);
    private readonly List<ITypeNameRule> _typeNameRules = new();

    public static ITypeHelper Instance { get; } = CreateDefaultTypeHelper();

    public void AddRule(ITypeNameRule rule)
    {
        Guard.NotNull(rule);

        _lock.EnterWriteLock();
        try
        {
            _typeNameRules.Add(rule);
            _typeNameRules.Sort((r1, r2) => r1.Priority.CompareTo(r2.Priority));
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    string? ITypeHelper.TryGetNameOf(Type type, NameOfTypeOptions options)
    {
        ArgumentNullException.ThrowIfNull(type);

        _lock.EnterReadLock();
        try
        {
            return _typeNameRules.Select(r => r.TryGetNameOfType(type, this, options)).FirstOrDefault(n => !(n is null));
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    private static ITypeHelper CreateDefaultTypeHelper()
    {
        var result = new TypeHelper();
        result.AddRule(new GenericTypeNameRule());
        result.AddRule(new NormalTypeNameRule());
        result.AddRule(new NullableTypeNameRule());
        result.AddRule(new CSharpKeywordTypeNameRule());
        return result;
    }
}