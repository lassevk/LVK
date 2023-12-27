using System.Runtime.CompilerServices;

namespace LVK.Core.Extensions;

public readonly struct CancellationTokenAwaiter : ICriticalNotifyCompletion
{
    private readonly CancellationToken _cancellationToken;

    public CancellationTokenAwaiter(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    public object GetResult()
    {
        if (IsCompleted)
            throw new OperationCanceledException();

        throw new InvalidOperationException("The cancellation token is still pending");
    }

    public bool IsCompleted => _cancellationToken.IsCancellationRequested;

    public void OnCompleted(Action continuation) => _cancellationToken.Register(continuation);

    public void UnsafeOnCompleted(Action continuation) => _cancellationToken.Register(continuation);
}