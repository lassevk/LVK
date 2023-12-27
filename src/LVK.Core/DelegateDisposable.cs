namespace LVK.Core;

public sealed class DelegateDisposable : IDisposable
{
    private Action? _disposeDelegate;

    public DelegateDisposable(Action disposeDelegate)
    {
        Guard.NotNull(disposeDelegate);

        _disposeDelegate = disposeDelegate;
    }

    public void Dispose()
    {
        Interlocked.Exchange(ref _disposeDelegate, null)?.Invoke();
    }
}