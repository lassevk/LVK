using System.Runtime.CompilerServices;

namespace LVK.Data.BlobStorage;

public interface IBlobStorage
{
    Task<string> SaveAsync<T>(T instance, CancellationToken cancellationToken = default);
    Task<T?> TryLoadAsync<T>(string reference, CancellationToken cancellationToken = default);

    async Task<T> LoadAsync<T>(string reference, CancellationToken cancellationToken = default)
    {
        T? value = await TryLoadAsync<T>(reference, cancellationToken);
        if (value is null)
            throw new BlobNotFoundException("The specified blob reference does not exist in the blob storage", reference);

        return value;
    }

    Task SetTagAsync(string name, string reference, CancellationToken cancellationToken = default);
    Task<string?> TryGetTagAsync(string name, CancellationToken cancellationToken = default);

    async Task<string> GetTagAsync(string name, CancellationToken cancellationToken = default)
    {
        string? reference = await TryGetTagAsync(name, cancellationToken);
        if (reference is null)
            throw new TagNotFoundException("The specified tag does not exist in the blob storage", name);

        return reference;
    }

    IAsyncEnumerable<string> EnumerateTagsAsync(CancellationToken cancellationToken = default) => EnumerateTagsAsync("", true, cancellationToken);
    IAsyncEnumerable<string> EnumerateTagsAsync(bool recursive, CancellationToken cancellationToken = default) => EnumerateTagsAsync("", recursive, cancellationToken);

    IAsyncEnumerable<string> EnumerateTagsAsync(string namePrefix, CancellationToken cancellationToken = default) => EnumerateTagsAsync(namePrefix, true, cancellationToken);
    IAsyncEnumerable<string> EnumerateTagsAsync(string namePrefix, bool recursive, CancellationToken cancellationToken = default);
}