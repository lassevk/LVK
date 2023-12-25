namespace LVK;

/// <summary>
/// Implement <see cref="IDisposable"/> by invoking an <see cref="Action"/> delegate when disposed.
/// </summary>
public sealed class DelegateDisposable : IDisposable
{
    private Action? _disposeDelegate;

    /// <summary>
    /// Constructs a new instance of <see cref="DelegateDisposable"/> for the specified <see cref="Action"/>
    /// delegate.
    /// </summary>
    /// <param name="disposeDelegate">
    /// The <see cref="Action"/> delegate that will be invoked when this instance is disposed.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="disposeDelegate"/> is <c>null</c>.
    /// </exception>
    public DelegateDisposable(Action disposeDelegate)
    {
        Guard.NotNull(disposeDelegate);

        _disposeDelegate = disposeDelegate;
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Interlocked.Exchange(ref _disposeDelegate, null)?.Invoke();
    }
}