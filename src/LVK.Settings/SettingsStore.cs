using System.Text;
using System.Text.Json;

namespace LVK.Settings;

internal class SettingsStore : ISettingsStore
{
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

    public async Task SaveAsync<T>(string name, T settings, CancellationToken cancellationToken = default)
        where T : class, new()
    {
        string filePath = GetSettingsFilePath(name);
        string json = JsonSerializer.Serialize(settings);
        await File.WriteAllTextAsync(filePath, json, Encoding.UTF8, cancellationToken);
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