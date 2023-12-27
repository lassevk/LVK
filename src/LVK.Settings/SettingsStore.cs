using System.Buffers;
using System.Text;
using System.Text.Json;

using LVK.Core;
using LVK.Core.Caching;
using LVK.Typed;

namespace LVK.Settings;

internal class SettingsStore : ISettingsStore
{
    private readonly ITypeHelper _typeHelper;

    private Lazy<SearchValues<char>> _invalidNameChars = new(GetInvalidNameChars);
    private AutoWeakCache<Type, string> _nameCache;

    private static SearchValues<char> GetInvalidNameChars()
    {
        var invalidChars = string.Join("", Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars()));
        return SearchValues.Create(invalidChars);
    }

    public SettingsStore(ITypeHelper typeHelper)
    {
        Guard.NotNull(typeHelper);

        _typeHelper = typeHelper;
        _nameCache = WeakCache.Create<Type, string>(CalculateName);
    }

    public async Task<T> LoadAsync<T>(string name, CancellationToken cancellationToken = default)
        where T : class, new()
    {
        string filePath = GetSettingsFilePath(name);
        try
        {
            string json = await File.ReadAllTextAsync(filePath, Encoding.UTF8, cancellationToken);
            return JsonSerializer.Deserialize<T>(json) ?? new T();
        }
        catch (FileNotFoundException)
        {
            return new T();
        }
    }

    public Task<T> LoadAsync<T>(CancellationToken cancellationToken = default)
        where T : class, new()
        => LoadAsync<T>(CalculateName(typeof(T)), cancellationToken);

    public async Task SaveAsync<T>(string name, T settings, CancellationToken cancellationToken = default)
        where T : class, new()
    {
        string filePath = GetSettingsFilePath(name);
        string json = JsonSerializer.Serialize(settings);
        await File.WriteAllTextAsync(filePath, json, Encoding.UTF8, cancellationToken);
    }

    public Task SaveAsync<T>(T settings, CancellationToken cancellationToken = default)
        where T : class, new()
        => SaveAsync(_nameCache.GetValue(typeof(T))!, settings, cancellationToken);

    private string CalculateName(Type type)
    {
        var sb = new StringBuilder();
        foreach (char c in _typeHelper.NameOf(type))
        {
            if (!_invalidNameChars.Value.Contains(c))
                sb.Append(c);
        }

        return sb.ToString();
    }

    private string GetSettingsFilePath(string name)
    {
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolderName = Path.GetFileNameWithoutExtension(Environment.ProcessPath)!;

        string folderPath = Path.Combine(appData, "LVK", "Settings", appFolderName);
        Directory.CreateDirectory(folderPath);

        return Path.Combine(folderPath, name + ".json");
    }
}