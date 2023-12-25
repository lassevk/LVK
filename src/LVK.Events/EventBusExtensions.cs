namespace LVK.Events;

/// <summary>
/// This class provides extension methods for <see cref="IEventBus"/>.
/// </summary>
public static class EventBusExtensions
{
    /// <summary>
    /// Subscribe to an event of type <typeparamref name="T"/>. If an event of the given type
    /// is published, the subscriber delegate will be invoked.
    /// </summary>
    /// <param name="eventBus">
    /// The <see cref="IEventBus"/> this extension method will operate on.
    /// </param>
    /// <param name="group">
    /// A group discriminator, can be left <c>null</c> for global events. This parameter can be used
    /// to publish events only applicable to a certain user, or section of the application, or similar.
    /// </param>
    /// <param name="subscriber">
    /// The delegate that will be invoked when an event of the given type is published.
    /// </param>
    /// <typeparam name="T">
    /// The type of events to subscribe to.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IDisposable"/> reference that can be used to unsubscribe from the event at
    /// a later time. Note that the return type is nullable, which will
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="subscriber"/> is <c>null</c>.
    /// </exception>
    public static IDisposable? Subscribe<T>(this IEventBus eventBus, object? group, Func<T, Task> subscriber) => eventBus.Subscribe<T>(group, (message, cancellationToken) => subscriber(message));

    /// <summary>
    /// Subscribe to an event of type <typeparamref name="T"/>. If an event of the given type
    /// is published, the subscriber delegate will be invoked.
    /// </summary>
    /// <param name="eventBus">
    /// The <see cref="IEventBus"/> this extension method will operate on.
    /// </param>
    /// <param name="subscriber">
    /// The delegate that will be invoked when an event of the given type is published.
    /// </param>
    /// <typeparam name="T">
    /// The type of events to subscribe to.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IDisposable"/> reference that can be used to unsubscribe from the event at
    /// a later time. Note that the return type is nullable, which will
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="subscriber"/> is <c>null</c>.
    /// </exception>
    public static IDisposable? Subscribe<T>(this IEventBus eventBus, Func<T, Task> subscriber) => eventBus.Subscribe<T>(null, (message, cancellationToken) => subscriber(message));

    /// <summary>
    /// Subscribe to an event of type <typeparamref name="T"/>. If an event of the given type
    /// is published, the subscriber delegate will be invoked.
    /// </summary>
    /// <param name="eventBus">
    /// The <see cref="IEventBus"/> this extension method will operate on.
    /// </param>
    /// <param name="subscriber">
    /// The delegate that will be invoked when an event of the given type is published.
    /// </param>
    /// <typeparam name="T">
    /// The type of events to subscribe to.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IDisposable"/> reference that can be used to unsubscribe from the event at
    /// a later time. Note that the return type is nullable, which will
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="subscriber"/> is <c>null</c>.
    /// </exception>
    public static IDisposable? Subscribe<T>(this IEventBus eventBus, Func<T, CancellationToken, Task> subscriber) => eventBus.Subscribe(null, subscriber);

    /// <summary>
    /// Subscribe to an event of type <typeparamref name="T"/>. If an event of the given type
    /// is published, the subscriber delegate will be invoked.
    /// </summary>
    /// <param name="eventBus">
    /// The <see cref="IEventBus"/> this extension method will operate on.
    /// </param>
    /// <param name="group">
    /// A group discriminator, can be left <c>null</c> for global events. This parameter can be used
    /// to publish events only applicable to a certain user, or section of the application, or similar.
    /// </param>
    /// <param name="subscriber">
    /// The delegate that will be invoked when an event of the given type is published.
    /// </param>
    /// <typeparam name="T">
    /// The type of events to subscribe to.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IDisposable"/> reference that can be used to unsubscribe from the event at
    /// a later time. Note that the return type is nullable, which will
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="subscriber"/> is <c>null</c>.
    /// </exception>
    public static IDisposable? Subscribe<T>(this IEventBus eventBus, object? group, Action<T> subscriber)
        => eventBus.Subscribe<T>(group, (message, _) =>
        {
            subscriber(message);
            return Task.CompletedTask;
        });

    /// <summary>
    /// Subscribe to an event of type <typeparamref name="T"/>. If an event of the given type
    /// is published, the subscriber delegate will be invoked.
    /// </summary>
    /// <param name="eventBus">
    /// The <see cref="IEventBus"/> this extension method will operate on.
    /// </param>
    /// <param name="subscriber">
    /// The delegate that will be invoked when an event of the given type is published.
    /// </param>
    /// <typeparam name="T">
    /// The type of events to subscribe to.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IDisposable"/> reference that can be used to unsubscribe from the event at
    /// a later time. Note that the return type is nullable, which will
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="subscriber"/> is <c>null</c>.
    /// </exception>
    public static IDisposable? Subscribe<T>(this IEventBus eventBus, Action<T> subscriber)
        => eventBus.Subscribe<T>(null, (message, _) =>
        {
            subscriber(message);
            return Task.CompletedTask;
        });

    /// <summary>
    /// Publishes an event and returns a task that can be awaited to ensure all subscribers have
    /// processed the event.
    /// </summary>
    /// <param name="group">
    /// A group discriminator, can be left <c>null</c> for global events. This parameter can be used
    /// to publish events only applicable to a certain user, or section of the application, or similar.
    /// </param>
    /// <param name="eventBus">
    /// The <see cref="IEventBus"/> this extension method will operate on.
    /// </param>
    /// <param name="message">
    /// The message to publish.
    /// </param>
    /// <typeparam name="T">
    /// The type of event message that is being published.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Task"/> that will represent the act of publishing the event to all
    /// subscribers. It will complete when all subscribers have processed the event.
    /// </returns>
    public static Task PublishAsync<T>(this IEventBus eventBus, object? group, T message) => eventBus.PublishAsync(group, message, CancellationToken.None);

    /// <summary>
    /// Publishes an event and returns a task that can be awaited to ensure all subscribers have
    /// processed the event.
    /// </summary>
    /// <param name="eventBus">
    /// The <see cref="IEventBus"/> this extension method will operate on.
    /// </param>
    /// <param name="message">
    /// The message to publish.
    /// </param>
    /// <typeparam name="T">
    /// The type of event message that is being published.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Task"/> that will represent the act of publishing the event to all
    /// subscribers. It will complete when all subscribers have processed the event.
    /// </returns>
    public static Task PublishAsync<T>(this IEventBus eventBus, T message) => eventBus.PublishAsync(null, message, CancellationToken.None);

    /// <summary>
    /// Publishes an event and returns a task that can be awaited to ensure all subscribers have
    /// processed the event.
    /// </summary>
    /// <param name="eventBus">
    /// The <see cref="IEventBus"/> this extension method will operate on.
    /// </param>
    /// <param name="message">
    /// The message to publish.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel processing of events.
    /// </param>
    /// <typeparam name="T">
    /// The type of event message that is being published.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Task"/> that will represent the act of publishing the event to all
    /// subscribers. It will complete when all subscribers have processed the event.
    /// </returns>
    public static Task PublishAsync<T>(this IEventBus eventBus, T message, CancellationToken cancellationToken) => eventBus.PublishAsync(null, message, cancellationToken);
}