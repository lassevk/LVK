using System.ComponentModel;

namespace LVK;

/// <summary>
/// This type provided extension methods for <see cref="CancellationToken"/>.
/// </summary>
public static class CancellationTokenExtensions
{
    /// <summary>
    /// Create an awaiter for a <see cref="CancellationToken"/>, allowing it to be awaited.
    /// </summary>
    /// <param name="cancellationToken">
    /// The <see cref="CancellationToken"/> to create an awaiter for.
    /// </param>
    /// <returns>
    /// The awaiter for <see cref="CancellationToken"/>.
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static CancellationTokenAwaiter GetAwaiter(this CancellationToken cancellationToken) => new(cancellationToken);
}