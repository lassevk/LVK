using LVK.Core.App.Console;
using LVK.Events;
using LVK.Notifications.Pushover;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sandbox.ConsoleApp;

public class MainEntrypoint : IMainEntrypoint
{
    private readonly ILogger<MainEntrypoint> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEventBus _eventBus;

    public MainEntrypoint(ILogger<MainEntrypoint> logger, IConfiguration configuration, IEventBus eventBus)
    {
        _logger = logger;
        _configuration = configuration;
        _eventBus = eventBus;
    }

    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        await _eventBus.PublishAsync(new PushoverNotification("Test message").WithTitle("Test"), stoppingToken);
        return 0;
    }
}