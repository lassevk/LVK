namespace LVK;

/// <summary>
/// This exception is thrown from <see cref="Assertions.assert"/> if the specified expression does not
/// hold to be <c>true</c>.
/// </summary>
public class AssertionFailedException : InvalidOperationException
{
    /// <inheritdoc />
    public AssertionFailedException() { }

    /// <inheritdoc />
    public AssertionFailedException(string? message)
        : base(message) { }

    /// <inheritdoc />
    public AssertionFailedException(string? message, Exception? innerException)
        : base(message, innerException) { }
}