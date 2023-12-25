namespace LVK;

/// <summary>
/// This class implements a cache that uses <see cref="WeakReference"/> to hold the values. This means
/// the cache will not hold on to objects that otherwise would be eligible for garbage collection.
/// </summary>
/// <typeparam name="TKey">
/// The type of keys for the cache.
/// </typeparam>
/// <typeparam name="TValue">
/// The type of values in the cache.
/// </typeparam>
public class WeakCache<TKey, TValue>
    where TKey : notnull
    where TValue: class
{
    private readonly Dictionary<TKey, WeakReference> _cache;

    private readonly object _lock = new object();

    /// <summary>
    /// Constructs a new instance of <see cref="WeakCache{TKey,TValue}"/> with a
    /// default <see cref="IEqualityComparer{T}"/> for the keys.
    /// </summary>
    public WeakCache()
        : this(EqualityComparer<TKey>.Default)
    {

    }

    /// <summary>
    /// Constructs a new instance of <see cref="WeakCache{TKey,TValue}"/> with the
    /// specified <see cref="IEqualityComparer{T}"/> for the keys.
    /// </summary>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> to use when comparing keys.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="comparer"/> is <c>null</c>.
    /// </exception>
    public WeakCache(IEqualityComparer<TKey> comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer);

        _cache = new Dictionary<TKey, WeakReference>(comparer);
    }


    /// <summary>
    /// Try to obtain the value of the key from the cache.
    /// </summary>
    /// <param name="key">
    /// The key for the value to obtain.
    /// </param>
    /// <param name="value">
    /// If the method returns <c>true</c>, this parameter now contains the obtained value;
    /// otherwise this contains <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the value was obtained from the cache; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> is <c>null</c>.
    /// </exception>
    public bool TryGetValue(TKey key, out TValue? value)
    {
        (bool success, value) = TryGetValue(key);
        return success;
    }

    /// <summary>
    /// Try to obtain the value of the key from the cache.
    /// </summary>
    /// <param name="key">
    /// The key for the value to obtain.
    /// </param>
    /// <returns>
    /// A tuple with two properties, success and value. If the key was found in the cache,
    /// and the value has not been garbage collected, success will be <c>true</c>, and value will
    /// contain the obtained value. Otherwise the success property will be <c>false</c>,
    /// and value will be <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> is <c>null</c>.
    /// </exception>
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

    /// <summary>
    /// First checks to see if the cache contains a value for the given key, and if it does,
    /// returns that value. Otherwise, the delegate is called to construct a new value, which is
    /// then cached and returned.
    /// </summary>
    /// <param name="key">
    /// The key for the value to obtain.
    /// </param>
    /// <param name="factory">
    /// The delegate that will be used to construct a new value to cache and return.
    /// </param>
    /// <returns>
    /// Either the value from the cache, or the value from the delegate.
    /// </returns>
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