namespace LVK.Core.App.Console.Parameters.Options;

internal abstract class RoutableCommandOption
{
    protected RoutableCommandOption(string? longName, string? shortName, string? description)
    {
        Guard.Against(longName == null && shortName == null);

        LongName = longName;
        ShortName = shortName;
        Description = description ?? "No description provided";
    }

    public string? LongName { get; }
    public string? ShortName { get; }
    public string MainName => LongName != null ? $"--{LongName}" : $"-{ShortName}";

    public string AllNames()
    {
        List<string> parts = new();

        if (LongName != null)
            parts.Add($"--{LongName}");

        if (ShortName != null)
            parts.Add($"-{ShortName}");

        if (parts.Count == 0)
            throw new InvalidOperationException("Internal error: An option with no names is not legal");

        return string.Join(", ", parts);
    }

    public string Description { get; }

    public abstract RoutableCommandOptionArgumentsType ArgumentsType { get; }

    public abstract void Parse(string argument);
    public abstract void EndOfArguments();

    public abstract void Inject(object target);

    public IEnumerable<string> GetHelpText()
    {
        string argumentsHelp = ArgumentsType switch
        {
            RoutableCommandOptionArgumentsType.None => "",
            RoutableCommandOptionArgumentsType.One  => ": value",
            RoutableCommandOptionArgumentsType.Many => ": value [value ...]",
            _                                       => throw new InvalidOperationException($"Unable to provide help text for {nameof(RoutableCommandOptionArgumentsType)}.{ArgumentsType}"),
        };
        yield return $"{AllNames()}{argumentsHelp} - {Description}";

        foreach (string line in GetValueHelpText())
            yield return "  " + line;
    }

    protected virtual IEnumerable<string> GetValueHelpText() => Array.Empty<string>();
}