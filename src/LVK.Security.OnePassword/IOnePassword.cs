namespace LVK.Security.OnePassword;

public interface IOnePassword
{
    Task<OnePasswordItem?> TryGetAsync(string id, CancellationToken cancellationToken);

    async Task<OnePasswordItem> GetAsync(string id, CancellationToken cancellationToken)
    {
        OnePasswordItem? item = await TryGetAsync(id, cancellationToken);
        if (item is null)
            throw new InvalidOperationException($"Unable to retrieve 1Password item with id '{id}'");

        return item;
    }
}