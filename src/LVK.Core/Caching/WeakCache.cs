namespace LVK.Core.Caching;

public class WeakCache<TKey, TValue>
    where TKey : notnull
    where TValue: class
{
    private readonly Dictionary<TKey, WeakReference> _cache;

    private readonly object _lock = new object();

    public WeakCache()
        : this(EqualityComparer<TKey>.Default)
    {

    }

    public WeakCache(IEqualityComparer<TKey> comparer)
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

    public TValue? GetOrAddValue(TKey key, Func<TKey, TValue?> factory)
    {
        lock (_lock)
        {
            (bool success, TValue? value) = TryGetValue(key);
            if (success)
                return value;

            value = factory(key);
            Guard.Assume(value != null);

            _cache[key] = new WeakReference(value);
            return value;
        }
    }
}