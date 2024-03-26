using System.Collections;
using System.Reflection;
using System.Text.Json;

using LVK.Core.App.Console;
using LVK.Data.BlobStorage;
using LVK.Security.OnePassword;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sandbox.ConsoleApp;

public class MainEntrypoint : IMainEntrypoint
{
    private readonly IConfiguration _configuration;

    public MainEntrypoint(ILogger<MainEntrypoint> logger, IBlobStorageFactory blobStorageFactory, IOnePassword onePassword, IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        Console.WriteLine(_configuration["SomeSetting"]);

        return 0;
    }
}