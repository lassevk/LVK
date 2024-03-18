namespace LVK.Data.BlobStorage;

public interface IBlobStorageFactory
{
    Task<IBlobStorage> OpenAsync(string folderPath, CancellationToken cancellation);
}