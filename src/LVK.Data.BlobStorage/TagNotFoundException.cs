namespace LVK.Data.BlobStorage;

public class TagNotFoundException : InvalidOperationException
{
    public TagNotFoundException(string message, string name)
        : base(message)
    {
        Name = name;
    }

    public string Name { get; }
}