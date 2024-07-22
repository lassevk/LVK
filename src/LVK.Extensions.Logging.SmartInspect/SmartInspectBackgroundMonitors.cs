using Gurock.SmartInspect;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LVK.Extensions.Logging.SmartInspect;

internal class SmartInspectBackgroundMonitors : IHostedService
{
    private readonly Session _session;
    private CancellationTokenSource? _cancellationTokenSource;
    private SmartInspectLoggerConfiguration _configuration;

    public SmartInspectBackgroundMonitors(Session session, IOptionsMonitor<SmartInspectLoggerConfiguration> configurationMonitor)
    {
        _session = session;
        _configuration = configurationMonitor.CurrentValue;
        configurationMonitor.OnChange(configuration =>
        {
            _configuration = configuration;
            StartStopMonitors();
        });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        StartStopMonitors();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource?.Cancel();
        return Task.CompletedTask;
    }

    private void StartStopMonitors()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        if (_configuration.Monitors.Cpu)
            _ = new CpuMonitor(_session, _cancellationTokenSource.Token).Run();

        if (_configuration.Monitors.GC)
            GarbageCollectionMonitor.Start(_session, _cancellationTokenSource.Token);

        if (_configuration.Monitors.Memory)
            _ = new MemoryMonitor(_session, _cancellationTokenSource.Token).Run();
    }
}