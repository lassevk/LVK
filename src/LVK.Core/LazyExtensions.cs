using System.ComponentModel;

namespace LVK;

/// <summary>
/// This type provided extension methods for <see cref="Lazy{T}"/>.
/// </summary>
public static class LazyExtensions
{
    /// <summary>
    /// Create an awaiter for a <see cref="Lazy{T}"/>, allowing it to be awaited.
    /// </summary>
    /// <param name="lazy">
    /// The <see cref="Lazy{T}"/> object to create an awaiter for.
    /// </param>
    /// <typeparam name="T">
    /// Specifies the type of element being lazily initialized.
    /// </typeparam>
    /// <returns>
    /// The awaiter for <see cref="Lazy{T}"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="lazy"/> is <c>null</c>.
    /// </exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static LazyAwaiter<T> GetAwaiter<T>(this Lazy<T> lazy)
    {
        ArgumentNullException.ThrowIfNull(lazy);

        return new LazyAwaiter<T>(lazy);
    }
}