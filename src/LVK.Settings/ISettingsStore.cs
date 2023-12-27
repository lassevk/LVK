namespace LVK.Settings;

public interface ISettingsStore
{
    Task<T> LoadAsync<T>(string name, CancellationToken cancellationToken = default)
        where T : class, new();

    Task SaveAsync<T>(string name, T settings, CancellationToken cancellationToken = default)
        where T : class, new();
}