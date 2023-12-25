using System.Runtime.CompilerServices;

namespace LVK;

/// <summary>
/// This type handles awaiting on <see cref="CancellationToken"/>. It is created
/// and returned from <see cref="CancellationTokenExtensions.GetAwaiter"/>.
/// </summary>
public readonly struct CancellationTokenAwaiter : ICriticalNotifyCompletion
{
    private readonly CancellationToken _cancellationToken;

    /// <summary>
    /// Construct a new <see cref="CancellationTokenAwaiter"/> for the <paramref name="cancellationToken"/> instance.
    /// </summary>
    /// <param name="cancellationToken">
    /// The <see cref="CancellationToken"/> that should be awaitable.
    /// </param>
    public CancellationTokenAwaiter(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    /// <summary>
    /// This method is not supported for <see cref="CancellationToken"/>.
    /// </summary>
    /// <exception cref="OperationCanceledException">
    /// The <see cref="CancellationToken"/> was cancelled.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="CancellationToken"/> has not yet been cancelled.
    /// </exception>
    public object GetResult()
    {
        if (IsCompleted)
            throw new OperationCanceledException();

        throw new InvalidOperationException("The cancellation token is still pending");
    }

    /// <summary>
    /// Get a value indicating whether the awaited <see cref="CancellationToken"/> has been cancelled.
    /// </summary>
    public bool IsCompleted => _cancellationToken.IsCancellationRequested;

    /// <inheritdoc cref="INotifyCompletion.OnCompleted"/>
    public void OnCompleted(Action continuation) => _cancellationToken.Register(continuation);

    /// <inheritdoc cref="ICriticalNotifyCompletion.UnsafeOnCompleted"/>
    public void UnsafeOnCompleted(Action continuation) => _cancellationToken.Register(continuation);
}