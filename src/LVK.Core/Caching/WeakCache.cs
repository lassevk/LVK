using System.Collections;

namespace LVK.Core.Caching;

public static class WeakCache
{
    public static ManualWeakCache<TKey, TValue> Create<TKey, TValue>()
        where TKey : notnull
        where TValue: class
        => Create<TKey, TValue>(EqualityComparer<TKey>.Default);

    public static ManualWeakCache<TKey, TValue> Create<TKey, TValue>(IEqualityComparer<TKey> comparer)
        where TKey : notnull
        where TValue : class
        => new ManualWeakCache<TKey, TValue>(comparer);

    public static AutoWeakCache<TKey, TValue> Create<TKey, TValue>(Func<TKey, TValue> calculate)
        where TKey : notnull
        where TValue : class
        => Create<TKey, TValue>(calculate, EqualityComparer<TKey>.Default);

    public static AutoWeakCache<TKey, TValue> Create<TKey, TValue>(Func<TKey, TValue> calculate, IEqualityComparer<TKey> comparer)
        where TKey : notnull
        where TValue : class
        => new AutoWeakCache<TKey, TValue>(calculate, comparer);
}

public abstract class WeakCache<TKey, TValue>
    where TKey : notnull
    where TValue : class
{
    private readonly Dictionary<TKey, WeakReference> _cache;

    private readonly object _lock = new object();

    protected WeakCache(IEqualityComparer<TKey> comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer);

        _cache = new Dictionary<TKey, WeakReference>(comparer);
    }

    public bool TryGetValue(TKey key, out TValue? value)
    {
        (bool success, value) = TryGetValue(key);
        return success;
    }

    public (bool success, TValue? value) TryGetValue(TKey key)
    {
        ArgumentNullException.ThrowIfNull(key);

        lock (_lock)
        {
            if (!_cache.TryGetValue(key, out WeakReference? weakReference))
                return (false, default);

            object? value = weakReference.Target;
            if (weakReference.IsAlive)
                return (true, (TValue?)value);

            _cache.Remove(key);
            return (false, default);
        }
    }

    protected void Set(TKey key, TValue value)
    {
        lock (_lock)
        {
            _cache[key] = new WeakReference(value);
        }
    }

    protected TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
    {
        lock (_lock)
        {
            (bool success, TValue? value) = TryGetValue(key);
            if (success)
                return value!;

            value = factory(key);
            Guard.Assume(value != null);

            _cache[key] = new WeakReference(value);
            return value;
        }
    }
}