using System.Numerics;

namespace LVK.Extensions.Logging.SmartInspect;

internal class CircularQueue<T>
    where T : INumber<T>
{
    private readonly LinkedList<(DateTime timestamp, T value)> _items = new();

    public void Append(T value)
    {
        _items.AddLast((DateTime.Now, value));
    }

    public T Diff()
    {
        if (_items.Count < 2)
            return T.Zero;

        (T first, T last)? firstLast = Get();
        if (firstLast == null)
            return T.Zero;

        return firstLast.Value.last - firstLast.Value.first;

    }

    public (T first, T last)? Get()
    {
        if (_items.Count == 0)
            return null;

        (DateTime timestamp, T value) last = _items.Last();
        DateTime cutoff = last.timestamp.AddSeconds(-1);

        while (_items.First().timestamp < cutoff)
            _items.RemoveFirst();

        (DateTime timestamp, T value) first = _items.First();

        return (first.value, last.value);
    }
}