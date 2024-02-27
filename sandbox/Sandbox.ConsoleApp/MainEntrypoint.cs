using LVK.Core.App.Console;
using LVK.Events;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sandbox.ConsoleApp;

public class MainEntrypoint : IMainEntrypoint
{
    private readonly ILogger<MainEntrypoint> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEventBus _eventBus;
    private readonly IDbContextFactory<TestDbContext> _dbContextFactory;

    public MainEntrypoint(ILogger<MainEntrypoint> logger, IConfiguration configuration, IEventBus eventBus, IDbContextFactory<TestDbContext> dbContextFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _eventBus = eventBus;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        await using TestDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(stoppingToken);

        foreach (TestItem item in dbContext.Items!)
            Console.WriteLine(item.Value);

        Console.WriteLine("Done");
        return 0;
    }
}