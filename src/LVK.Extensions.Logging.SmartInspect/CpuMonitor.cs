using System.Diagnostics;

using Gurock.SmartInspect;

namespace LVK.Extensions.Logging.SmartInspect;

internal class CpuMonitor
{
    private readonly Session _session;
    private readonly CancellationToken _cancellationToken;

    private readonly CircularQueue<double> _measurements = new();

    public CpuMonitor(Session session, CancellationToken cancellationToken)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
        _cancellationToken = cancellationToken;
    }

    public async Task Run()
    {
        using var process = Process.GetCurrentProcess();
        while (!_cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(333, _cancellationToken);
            _measurements.Append(process.TotalProcessorTime.TotalMilliseconds);
            _session.WatchInt("cpu.total", (int)(_measurements.Diff() * 100.0));
        }
    }
}