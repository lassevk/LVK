namespace LVK.Core;

public record NoOperationDisposable : IDisposable
{
    public void Dispose()
    {
        // Do nothing here
    }
}