using Nito.AsyncEx;

namespace LVK.Data.Processing;

public readonly record struct ProcessingEntity()
{
    private readonly AsyncReaderWriterLock _lock = new();
    private readonly Dictionary<Type, object> _components = new();

    public async Task<T?> TryGetComponentAsync<T>(CancellationToken cancellationToken = default)
        where T : class
    {
        using IDisposable acquiredLock = await _lock.ReaderLockAsync(cancellationToken);
        _components.TryGetValue(typeof(T), out object? component);
        return (T?)component;
    }

    public async Task AddComponentAsync<T>(T component, CancellationToken cancellationToken = default)
        where T : class
    {
        using IDisposable acquiredLock = await _lock.WriterLockAsync(cancellationToken);
        _components.Add(typeof(T), component);
    }

    public async Task SetComponentAsync<T>(T component, CancellationToken cancellationToken = default)
        where T : class
    {
        using IDisposable acquiredLock = await _lock.WriterLockAsync(cancellationToken);
        _components[typeof(T)] = component;
    }
}