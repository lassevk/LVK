namespace LVK;

/// <summary>
/// Implement <see cref="IAsyncDisposable"/> by invoking an <see cref="Func{ValueTask}"/> delegate when disposed.
/// </summary>
public sealed class AsyncDelegateDisposable : IAsyncDisposable
{
    private Func<ValueTask>? _disposeDelegate;

    /// <summary>
    /// Constructs a new instance of <see cref="AsyncDelegateDisposable"/> for the specified <see cref="Func{ValueTask}"/>
    /// delegate.
    /// </summary>
    /// <param name="disposeDelegate">
    /// The <see cref="Func{ValueTask}"/> delegate that will be invoked when this instance is disposed.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="disposeDelegate"/> is <c>null</c>.
    /// </exception>
    public AsyncDelegateDisposable(Func<ValueTask> disposeDelegate)
    {
        Guard.NotNull(disposeDelegate);
        _disposeDelegate = disposeDelegate;
    }

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
    public ValueTask DisposeAsync()
    {
        Func<ValueTask>? action = Interlocked.Exchange(ref _disposeDelegate, null);
        return action?.Invoke() ?? ValueTask.CompletedTask;
    }
}