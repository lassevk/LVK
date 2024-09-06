namespace LVK.Core.App.Console;

public class ConsoleLine
{
    public void Set(string value)
    {
        if (Current == value)
            return;

        System.Console.Out.MoveToStartOfLine().Write(value);
        System.Console.Out.ClearToEndOfLine();

        Current = value;
    }

    public void Clear() => Set("");

    public string Current { get; private set; } = "";
}