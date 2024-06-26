namespace LVK.Events;

public interface IEventBus
{
    IDisposable Subscribe<T>(object? group, IEventSubscriber<T> subscriber);
    IDisposable Subscribe<T>(object? group, Func<T, CancellationToken, Task> subscriber);

    Task PublishAsync<T>(object? group, T message, CancellationToken cancellationToken);
}