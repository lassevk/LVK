namespace LVK.Core;

public class AssertionFailedException : InvalidOperationException
{
    public AssertionFailedException() { }

    public AssertionFailedException(string? message)
        : base(message) { }

    public AssertionFailedException(string? message, Exception? innerException)
        : base(message, innerException) { }
}