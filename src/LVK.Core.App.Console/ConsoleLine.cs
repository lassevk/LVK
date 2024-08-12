namespace LVK.Core.App.Console;

public class ConsoleLine
{
    public void Set(string value)
    {
        if (Current == value)
            return;

        ClearExcess(Current, value);
        RewriteNew(Current, value);
        AppendExtra(Current, value);
        Current = value;
    }

    public void Clear() => Set("");

    public string Current { get; private set; } = "";

    private void AppendExtra(string current, string value)
    {
        if (value.Length <= current.Length)
            return;

        System.Console.Write(value[current.Length ..]);
    }

    private void RewriteNew(string current, string value)
    {
        int toCheck = Math.Min(current.Length, value.Length);
        int? firstChanged = Enumerable.Range(0, toCheck).Select(idx => (int?)idx).FirstOrDefault(idx => current[idx!.Value] != value[idx.Value]);
        if (!firstChanged.HasValue)
            return;

        for (int index = current.Length; index > firstChanged.Value; index--)
            System.Console.Write('\u0008');
        System.Console.Write(value[firstChanged.Value .. toCheck]);
    }

    private void ClearExcess(string current, string value)
    {
        if (current.Length <= value.Length)
            return;

        int toRemove = current.Length - value.Length;
        for (var index = 0; index < toRemove; index++)
            System.Console.Write("\u0008 \u0008");
    }
}