using LVK.Extensions.Bootstrapping.Console;

using Microsoft.Extensions.Logging;

namespace Sandbox.Console;

public class MainEntrypoint : IMainEntrypoint
{
    private readonly ILogger<MainEntrypoint> _logger;

    public MainEntrypoint(ILogger<MainEntrypoint> logger)
    {
        _logger = logger;
    }

    public Task<int> RunAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Entering RunAsync");
        System.Console.WriteLine("Here");

        return Task.FromResult(0);
    }
}