using LVK.Core.App.Console;
using LVK.Data.BlobStorage;

using Microsoft.Extensions.Logging;

namespace Sandbox.ConsoleApp;

public class MainEntrypoint : IMainEntrypoint
{
    private readonly ILogger<MainEntrypoint> _logger;
    private readonly IBlobStorageFactory _blobStorageFactory;

    public MainEntrypoint(ILogger<MainEntrypoint> logger, IBlobStorageFactory blobStorageFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _blobStorageFactory = blobStorageFactory ?? throw new ArgumentNullException(nameof(blobStorageFactory));
    }

    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        IBlobStorage storage = await _blobStorageFactory.OpenAsync("/Users/lassevk/Temp", stoppingToken);

        // string reference = await storage.SaveAsync("This is a test", stoppingToken);
        // await storage.SetTagAsync("test/main", reference, stoppingToken);
        //
        // reference = await storage.GetTagAsync("test/main", stoppingToken);
        // var obj = await storage.LoadAsync<string>(reference, stoppingToken);

        await foreach (string name in storage.EnumerateTagsAsync(false, stoppingToken))
        {
            Console.WriteLine(name);
        }

        return 0;
    }
}

public class Test
{
    public int id { get; set; }
    public string value { get; set; }
}