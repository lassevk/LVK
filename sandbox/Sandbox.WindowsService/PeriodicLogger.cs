using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sandbox.WindowsService;

public class PeriodicLogger(ILogger<PeriodicLogger> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Now = {Now}", DateTime.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}