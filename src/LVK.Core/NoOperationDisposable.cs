namespace LVK;

/// <summary>
/// This <see cref="IDisposable"/> type does nothing. Can be used
/// instead of returning <c>null</c> from methods that should
/// return something that can be disposed, but for some reason
/// has nothing to dispose of/do.
/// </summary>
public record NoOperationDisposable : IDisposable
{
    /// <inheritdoc />
    public void Dispose()
    {
        // Do nothing here
    }
}