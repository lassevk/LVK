using System.Collections.Concurrent;

using LVK.Core;

namespace LVK.Events;

internal class EventBus : IEventBus
{
    private readonly object _globalGroup = new();
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, HashSet<object>>> _subscribers = new();

    IDisposable IEventBus.Subscribe<T>(object? group, Func<T, CancellationToken, Task> subscriber)
    {
        ArgumentNullException.ThrowIfNull(subscriber);

        Type messageType = typeof(T);
        group ??= _globalGroup;

        ConcurrentDictionary<object, HashSet<object>> typeSubscribers = _subscribers.GetOrAdd(messageType, _ => new());
        HashSet<object> groupSubscribers = typeSubscribers.GetOrAdd(group, _ => new());

        lock (groupSubscribers)
        {
            if (!groupSubscribers.Add(subscriber))
                return new NoOperationDisposable();
        }

        return new DelegateDisposable(() =>
        {
            lock (groupSubscribers)
            {
                groupSubscribers.Remove(subscriber);
            }
        });
    }

    async Task IEventBus.PublishAsync<T>(object? group, T message, CancellationToken cancellationToken)
    {
        Type messageType = typeof(T);
        if (!_subscribers.TryGetValue(messageType, out ConcurrentDictionary<object, HashSet<object>>? typeSubscribers))
            return;

        group ??= _globalGroup;
        if (!typeSubscribers.TryGetValue(group, out HashSet<object>? groupSubscribers))
            return;

        List<object> subscribers;
        lock (groupSubscribers)
        {
            subscribers = groupSubscribers.ToList();
        }

        var tasks = new List<Task>();
        try
        {
            tasks.AddRange(subscribers.Select(subscriber => ((Func<T, CancellationToken, Task>)subscriber).Invoke(message, cancellationToken)));
        }
        finally
        {
            await Task.WhenAll(tasks);
        }
    }
}