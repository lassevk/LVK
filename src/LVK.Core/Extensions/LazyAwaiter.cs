using System.Runtime.CompilerServices;

namespace LVK.Core.Extensions;

public readonly struct LazyAwaiter<T> : INotifyCompletion
{
    private readonly Lazy<T> _lazy;

    public LazyAwaiter(Lazy<T> lazy)
    {
        _lazy = lazy ?? throw new ArgumentNullException(nameof(lazy));
    }

    public T GetResult() => _lazy.Value;

    public bool IsCompleted => true;

    public void OnCompleted(Action continuation) { }
}