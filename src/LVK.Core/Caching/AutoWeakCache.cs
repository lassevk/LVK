namespace LVK.Core.Caching;

public class AutoWeakCache<TKey, TValue> : WeakCache<TKey, TValue>
    where TKey : notnull
    where TValue: class
{
    private readonly Func<TKey,TValue> _calculate;

    public AutoWeakCache(Func<TKey, TValue> calculate, IEqualityComparer<TKey>? comparer = null)
        : base(comparer ?? EqualityComparer<TKey>.Default)
    {
        Guard.NotNull(calculate);

        _calculate = calculate;
    }

    public void Refresh(TKey key) => Set(key, _calculate(key));
    public TValue GetValue(TKey key) => GetOrAdd(key, _calculate);
}