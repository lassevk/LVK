namespace LVK.Core.Caching;

public class ManualWeakCache<TKey, TValue> : WeakCache<TKey, TValue>
    where TKey : notnull
    where TValue : class
{
    public ManualWeakCache(IEqualityComparer<TKey>? comparer = null)
        : base(comparer ?? EqualityComparer<TKey>.Default)
    {
    }

    public void SetValue(TKey key, TValue value) => Set(key, value);
    public TValue? GetOrAddValue(TKey key, Func<TKey, TValue> factory) => GetOrAdd(key, factory);
}