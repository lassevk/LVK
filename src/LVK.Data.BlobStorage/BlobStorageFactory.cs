namespace LVK.Data.BlobStorage;

internal class BlobStorageFactory : IBlobStorageFactory
{
    public async Task<IBlobStorage> OpenAsync(string folderPath, CancellationToken cancellation)
    {
        await Task.Yield();
        return new BlobStorage(folderPath);
    }
}