using System.Runtime.CompilerServices;

namespace LVK;

/// <summary>
/// This type handles awaiting on <see cref="Lazy{T}"/>. It is created
/// and returned from <see cref="LazyExtensions.GetAwaiter{T}"/>.
/// </summary>
/// <typeparam name="T">
/// Specifies the type of element being lazily initialized.
/// </typeparam>
public readonly struct LazyAwaiter<T> : INotifyCompletion
{
    private readonly Lazy<T> _lazy;

    /// <summary>
    /// Construct a new <see cref="LazyAwaiter{T}"/> for the <paramref name="lazy"/> instance.
    /// </summary>
    /// <param name="lazy">
    /// The <see cref="Lazy{T}"/> that should be awaitable.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="lazy"/> is <c>null</c>.
    /// </exception>
    public LazyAwaiter(Lazy<T> lazy)
    {
        _lazy = lazy ?? throw new ArgumentNullException(nameof(lazy));
    }

    /// <summary>
    /// Gets the lazily constructed value of the <see cref="Lazy{T}"/> that is being awaited.
    /// </summary>
    /// <returns>
    /// The value of the awaited <see cref="Lazy{T}"/>.
    /// </returns>
    public T GetResult() => _lazy.Value;

    /// <summary>
    /// We pretend the lazy object has already been initialized completely.
    /// </summary>
    public bool IsCompleted => true;

    /// <inheritdoc cref="INotifyCompletion.OnCompleted"/>
    public void OnCompleted(Action continuation) { }
}