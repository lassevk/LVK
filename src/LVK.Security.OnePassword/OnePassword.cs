using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

using LVK.Core.Extensions;

namespace LVK.Security.OnePassword;

internal class OnePassword : IOnePassword
{
    public async Task<OnePasswordItem?> TryGetAsync(string id, CancellationToken cancellationToken)
    {
        string json = await ExecuteAsync(cancellationToken, "item", "get", id, "--format", "json");
        // string json = await File.ReadAllTextAsync("/Users/lassevk/item.json", cancellationToken);
        return JsonSerializer.Deserialize<OnePasswordItem>(json, new JsonSerializerOptions
        {
            PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
        });
    }

    private async Task<string> ExecuteAsync(CancellationToken cancellationToken, params string[] arguments)
    {
        var psi = new ProcessStartInfo("op");
        psi.ArgumentList.AddRange(arguments);
        psi.CreateNoWindow = true;
        psi.UseShellExecute = false;

        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;

        using var process = Process.Start(psi);
        if (process is null)
            throw new InvalidOperationException();

        await process.WaitForExitAsync(cancellationToken);
        if (process.ExitCode != 0)
            throw new InvalidOperationException(await process.StandardError.ReadToEndAsync(cancellationToken));

        return await process.StandardOutput.ReadToEndAsync(cancellationToken);
    }
}