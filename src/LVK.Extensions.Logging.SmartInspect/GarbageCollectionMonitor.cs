using Gurock.SmartInspect;

namespace LVK.Extensions.Logging.SmartInspect;

internal class GarbageCollectionMonitor
{
    private static Session _session = default!;
    private static CancellationToken _token;

    private static string[] _generationNames = default!;
    private static int[] _trackers = default!;

    public static void Start(Session session, CancellationToken token)
    {
        _session = session;
        _token = token;
        _trackers = new int[GC.MaxGeneration + 1];
        _generationNames = Enumerable.Range(0, GC.MaxGeneration + 1).Select(i => $"gc.gen{i}").ToArray();

        _ = new GarbageCollectionMonitor();
    }

    private GarbageCollectionMonitor()
    {
    }

    ~GarbageCollectionMonitor()
    {
        if (_token.IsCancellationRequested)
            return;

        for (var index = 0; index <= GC.MaxGeneration; index++)
        {
            var collectionCount = GC.CollectionCount(index);
            if (collectionCount <= _trackers[index])
                continue;

            _trackers[index] = collectionCount;
            _session.Watch(_generationNames[index], 0);
            _session.Watch(_generationNames[index], 100);
            _session.Watch(_generationNames[index], 0);
        }

        _ = new GarbageCollectionMonitor();
    }
}