using Gurock.SmartInspect;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LVK.Extensions.Logging.SmartInspect;

internal class SmartInspectProcessLogger : IHostedService
{
    private readonly ILogger<SmartInspectProcessLogger> _logger;

    public SmartInspectProcessLogger(ILogger<SmartInspectProcessLogger> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        SiAuto.Main.EnterProcess();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        SiAuto.Main.LeaveProcess();
        return Task.CompletedTask;
    }
}