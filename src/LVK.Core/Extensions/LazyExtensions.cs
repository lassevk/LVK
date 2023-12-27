using System.ComponentModel;

namespace LVK.Core.Extensions;

public static class LazyExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static LazyAwaiter<T> GetAwaiter<T>(this Lazy<T> lazy)
    {
        ArgumentNullException.ThrowIfNull(lazy);

        return new LazyAwaiter<T>(lazy);
    }
}