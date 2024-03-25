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
    private readonly ILogger<MainEntrypoint> _logger;
    private readonly IBlobStorageFactory _blobStorageFactory;
    private readonly IOnePassword _onePassword;
    private readonly IConfiguration _configuration;

    public MainEntrypoint(ILogger<MainEntrypoint> logger, IBlobStorageFactory blobStorageFactory, IOnePassword onePassword, IConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _blobStorageFactory = blobStorageFactory ?? throw new ArgumentNullException(nameof(blobStorageFactory));
        _onePassword = onePassword ?? throw new ArgumentNullException(nameof(onePassword));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        OnePasswordItem item = await _onePassword.GetAsync(_configuration["ItemId"]!, stoppingToken);
        Console.WriteLine(item.Title);

        return 0;
    }
}