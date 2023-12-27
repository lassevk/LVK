namespace LVK.Core;

public sealed class AsyncDelegateDisposable : IAsyncDisposable
{
    private Func<ValueTask>? _disposeDelegate;

    public AsyncDelegateDisposable(Func<ValueTask> disposeDelegate)
    {
        Guard.NotNull(disposeDelegate);
        _disposeDelegate = disposeDelegate;
    }

    public ValueTask DisposeAsync()
    {
        Func<ValueTask>? action = Interlocked.Exchange(ref _disposeDelegate, null);
        return action?.Invoke() ?? ValueTask.CompletedTask;
    }
}