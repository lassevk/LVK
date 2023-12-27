namespace LVK.Events;

public static class EventBusExtensions
{
    public static IDisposable? Subscribe<T>(this IEventBus eventBus, object? group, Func<T, Task> subscriber) => eventBus.Subscribe<T>(group, (message, cancellationToken) => subscriber(message));

    public static IDisposable? Subscribe<T>(this IEventBus eventBus, Func<T, Task> subscriber) => eventBus.Subscribe<T>(null, (message, cancellationToken) => subscriber(message));

    public static IDisposable? Subscribe<T>(this IEventBus eventBus, Func<T, CancellationToken, Task> subscriber) => eventBus.Subscribe(null, subscriber);

    public static IDisposable? Subscribe<T>(this IEventBus eventBus, object? group, Action<T> subscriber)
        => eventBus.Subscribe<T>(group, (message, _) =>
        {
            subscriber(message);
            return Task.CompletedTask;
        });

    public static IDisposable? Subscribe<T>(this IEventBus eventBus, Action<T> subscriber)
        => eventBus.Subscribe<T>(null, (message, _) =>
        {
            subscriber(message);
            return Task.CompletedTask;
        });

    public static Task PublishAsync<T>(this IEventBus eventBus, object? group, T message) => eventBus.PublishAsync(group, message, CancellationToken.None);

    public static Task PublishAsync<T>(this IEventBus eventBus, T message) => eventBus.PublishAsync(null, message, CancellationToken.None);

    public static Task PublishAsync<T>(this IEventBus eventBus, T message, CancellationToken cancellationToken) => eventBus.PublishAsync(null, message, cancellationToken);
}