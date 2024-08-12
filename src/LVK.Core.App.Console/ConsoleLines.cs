namespace LVK.Core.App.Console;

public class ConsoleLines
{
    private readonly ConsoleLine[] _lines;
    private readonly object _lock = new();

    public ConsoleLines(int count)
    {
        Count = count;
        _lines = Enumerable.Range(0, count).Select(_ => new ConsoleLine()).ToArray();
        foreach (ConsoleLine line in _lines[..^1])
            System.Console.WriteLine();
    }

    public int Count { get; }

    private void GoToLine(int current, int next)
    {
        if (current == next)
            return;

        System.Console.Write(current > next ? $"\u001b[{current - next}A" : $"\u001b[{next - current}B");
        System.Console.Write($"\u001b[{_lines[next].Current.Length + 1}G");
    }

    public void Set(int index, string value)
    {
        lock (_lock)
        {
            if (_lines[index].Current == value)
                return;

            GoToLine(Count - 1, index);
            _lines[index].Set(value);
            GoToLine(index, Count - 1);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            GoToLine(Count - 1, 0);
            for (var index = 0; index < Count; index++)
            {
                GoToLine(Math.Max(0, index - 1), index);
                _lines[index].Clear();
            }
        }
    }

    public void ScrollUp(int index, int count)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index), $"index must be in the range 0..{_lines.Length}");

        if (count < 0 || index + count > Count)
            throw new ArgumentOutOfRangeException(nameof(count), $"count must be in the range 0..{Count - index}");

        if (count == 0)
            return;

        lock (_lock)
        {
            int current = Count - 1;
            for (var i = 0; i < count - 1; i++)
            {
                GoToLine(current, index + i);
                current = index + i;
                _lines[index + i].Set(_lines[index + i + 1].Current);
            }

            GoToLine(current, index + count - 1);

            _lines[index + count - 1].Clear();
            GoToLine(index + count - 1, Count - 1);
        }
    }

    public void Remove()
    {
        Clear();
        GoToLine(Count - 1, 0);
    }
}