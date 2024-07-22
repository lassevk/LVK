using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

using LVK.Core;

namespace LVK.Data.BlobStorage;

internal partial class BlobStorage : IBlobStorage
{
    private const string _objectFolderName = "objects";
    private const string _tagFolderName = "tags";

    private readonly char[] _digits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];

    private readonly string _folderPath;

    public BlobStorage(string folderPath)
    {
        _folderPath = folderPath ?? throw new ArgumentNullException(nameof(folderPath));
    }

    public async Task<string> SaveAsync<T>(T instance, CancellationToken cancellationToken)
    {
        if (instance is byte[] bytes)
            return await SaveBytesAsync(bytes, cancellationToken);

        if (instance is string s)
            return await SaveStringAsync(s, cancellationToken);

        string json = JsonSerializer.Serialize(instance);
        return await SaveStringAsync(json, cancellationToken);
    }

    public async Task<T?> TryLoadAsync<T>(string reference, CancellationToken cancellationToken)
    {
        Guard.Assert(ReferencePattern().IsMatch(reference));

        string filePath = ChecksumToFilePath(reference);
        if (!File.Exists(filePath))
            return default;

        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        await using var decompressionStream = new ZLibStream(fileStream, CompressionMode.Decompress);

        if (typeof(T) == typeof(byte[]) || typeof(T) == typeof(string))
        {
            var temp = new MemoryStream();
            await decompressionStream.CopyToAsync(temp, cancellationToken);

            if (typeof(T) == typeof(byte[]))
                return (T)(object)temp.ToArray();

            return (T)(object)Encoding.UTF8.GetString(temp.ToArray());
        }

        return await JsonSerializer.DeserializeAsync<T>(decompressionStream, JsonSerializerOptions.Default, cancellationToken);
    }

    public async Task SetTagAsync(string name, string reference, CancellationToken cancellationToken)
    {
        Guard.Assert(ReferencePattern().IsMatch(reference));

        string filePath = TagToFilePath(name);
        await File.WriteAllTextAsync(filePath, reference, cancellationToken);
    }

    public async Task<string?> TryGetTagAsync(string name, CancellationToken cancellationToken)
    {
        string filePath = TagToFilePath(name);
        if (!File.Exists(filePath))
            return null;

        return await File.ReadAllTextAsync(filePath, cancellationToken);
    }

    public async IAsyncEnumerable<string> EnumerateTagsAsync(string namePrefix, bool recursive, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.Yield();
        string tagFolderPath = TagToFilePath(namePrefix);

        await foreach (string tagName in EnumerateTagFolderAsync(Path.Combine(_folderPath, _tagFolderName).Length + 1, tagFolderPath, recursive, cancellationToken))
            yield return tagName;
    }

    private async Task<string> SaveStringAsync(string value, CancellationToken cancellationToken)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        return await SaveBytesAsync(bytes, cancellationToken);
    }

    private async Task<string> SaveBytesAsync(byte[] bytes, CancellationToken cancellationToken)
    {
        string checksum = GetChecksum(bytes);

        string filePath = ChecksumToFilePath(checksum);
        if (File.Exists(filePath))
            return checksum;

        await using var fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
        await using var compressionStream = new ZLibStream(fileStream, CompressionLevel.Optimal);
        await compressionStream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        return checksum;
    }

    private string ChecksumToFilePath(string checksum)
    {
        string folderPath = Path.Combine(_folderPath, _objectFolderName, checksum[..2]);
        Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, checksum[2..]);
        return filePath;
    }

    private string TagToFilePath(string name)
    {
        string[] parts = name.Split('/');
        string folderPath = Path.Combine([_folderPath, _tagFolderName, .. parts[..^1]]);
        Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, parts.Last());
        return filePath;
    }

    private string GetChecksum(byte[] bytes)
    {
        using var sha = SHA256.Create();
        byte[] checksum = sha.ComputeHash(bytes);
        Span<char> result = new char[64];
        for (int checksumIndex = 0,
            resultIndex = 0;
            checksumIndex < 32;
            checksumIndex++, resultIndex += 2)
        {
            byte b = checksum[checksumIndex];
            result[resultIndex] = _digits[b >> 16];
            result[resultIndex + 1] = _digits[b & 0x0f];
        }

        return new string(result);
    }

    private async IAsyncEnumerable<string> EnumerateTagFolderAsync(int prefixLength, string folderPath, bool recursive, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (recursive)
        {
            foreach (string subFolderName in Directory.GetDirectories(folderPath))
            {
                await foreach (string tagName in EnumerateTagFolderAsync(prefixLength, subFolderName, recursive, cancellationToken))
                    yield return tagName;
            }
        }

        foreach (string tagName in Directory.GetFiles(folderPath))
            yield return tagName[prefixLength..];
    }

    [GeneratedRegex("^[0-9a-f]{64}$")]
    private static partial Regex ReferencePattern();
}