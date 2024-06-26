using System.Collections.Concurrent;

using LVK.Core;

using Microsoft.Extensions.DependencyInjection;

namespace LVK.Events;

internal class EventBus : IEventBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly object _globalGroup = new();
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, HashSet<object>>> _subscribers = new();

    public EventBus(IServiceProvider serviceProvider)
    {
        Guard.NotNull(serviceProvider);

        _serviceProvider = serviceProvider;
    }

    IDisposable IEventBus.Subscribe<T>(object? group, IEventSubscriber<T> subscriber)
    {
        Guard.NotNull(subscriber);

        var wrapper = new WeakReference<IEventSubscriber<T>>(subscriber);
        IDisposable subscription = null!;

        subscription = ((IEventBus)this).Subscribe<T>(group, async (evt, ct) =>
        {
            if (!wrapper.TryGetTarget(out IEventSubscriber<T>? tempSubscriber))
            {
                // ReSharper disable once AccessToModifiedClosure
                subscription.Dispose();
                return;
            }

            await tempSubscriber.OnEvent(evt, ct);
        });

        return subscription;
    }

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
        Task serviceTask = PublishToServicesAsync(group, message, cancellationToken);
        Task delegateTask = PublishToDelegatesAsync(group, message, cancellationToken);

        await Task.WhenAll(serviceTask, delegateTask);
    }

    private async Task PublishToServicesAsync<T>(object? group, T message, CancellationToken cancellationToken)
    {
        List<IEventSubscriber<T>> subscribers = group != null
            ? _serviceProvider.GetKeyedServices<IEventSubscriber<T>>(group).ToList()
            : _serviceProvider.GetServices<IEventSubscriber<T>>().ToList();

        if (subscribers.Count == 0)
            return;

        var tasks = new List<Task>();
        try
        {
            tasks.AddRange(subscribers.Select(subscriber => subscriber.OnEvent(message, cancellationToken)));
        }
        finally
        {
            await Task.WhenAll(tasks);
        }
    }

    private async Task PublishToDelegatesAsync<T>(object? group, T message, CancellationToken cancellationToken)
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