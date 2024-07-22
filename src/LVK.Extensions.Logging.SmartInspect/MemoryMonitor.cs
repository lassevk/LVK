using System.Diagnostics;

using Gurock.SmartInspect;

namespace LVK.Extensions.Logging.SmartInspect;

internal class MemoryMonitor
{
    private readonly CircularQueue<long> _allocatedBytes = new();
    private readonly CancellationToken _cancellationToken;
    private readonly CircularQueue<long> _privateMemory = new();
    private readonly Session _session;
    private readonly CircularQueue<long> _virtualMemory = new();

    public MemoryMonitor(Session session, CancellationToken cancellationToken)
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

            long allocatedBytes = GC.GetTotalMemory(false);
            _allocatedBytes.Append(allocatedBytes);
            _session.WatchLong("mem.bytes.current", allocatedBytes);
            _session.WatchLong("mem.bytes.alloc", Math.Max(0, _allocatedBytes.Diff()));

            long processPrivateMemorySize64 = process.PrivateMemorySize64;
            _privateMemory.Append(processPrivateMemorySize64);
            _session.WatchLong("mem.private.current", processPrivateMemorySize64);
            _session.WatchLong("mem.private.alloc", Math.Max(0, _privateMemory.Diff()));

            long processVirtualMemorySize64 = process.VirtualMemorySize64;
            _virtualMemory.Append(processVirtualMemorySize64);
            _session.WatchLong("mem.virtual.current", processVirtualMemorySize64);
            _session.WatchLong("mem.virtual.alloc", Math.Max(0, _virtualMemory.Diff()));

            Test();
        }
    }

    private void Test()
    {
        for (var index = 0; index < Random.Shared.Next(32000); index++)
            GC.KeepAlive(new byte[Random.Shared.Next(65536)]);
    }
}