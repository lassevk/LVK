namespace LVK.Data.BlobStorage;

public class BlobNotFoundException : InvalidOperationException
{
    public BlobNotFoundException(string message, string reference)
        : base(message)
    {
        Reference = reference;
    }

    public string Reference { get; }
}