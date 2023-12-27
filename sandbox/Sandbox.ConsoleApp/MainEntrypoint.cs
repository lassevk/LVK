using LVK.Core.App.Console;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sandbox.ConsoleApp;

public class MainEntrypoint : IMainEntrypoint
{
    private readonly ILogger<MainEntrypoint> _logger;
    private readonly IConfiguration _configuration;

    public MainEntrypoint(ILogger<MainEntrypoint> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task<int> RunAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Entering RunAsync");
        Console.WriteLine("Here");
        Console.WriteLine("Value = " + _configuration["Value"]);

        return Task.FromResult(0);
    }
}