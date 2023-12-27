using System.ComponentModel;

namespace LVK.Core.Extensions;

public static class CancellationTokenExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static CancellationTokenAwaiter GetAwaiter(this CancellationToken cancellationToken) => new(cancellationToken);
}