namespace LVK.Core.App.Console.Parameters.Options;

internal class RoutableCommandStringsOption : RoutableCommandOption
{
    private readonly Action<object, string[]> _injectValue;
    private readonly List<string> _values = new();

    public RoutableCommandStringsOption(string? longName, string? shortName, string? description, Action<object, string[]> injectValue)
        : base(longName, shortName, description)
    {
        _injectValue = injectValue ?? throw new ArgumentNullException(nameof(injectValue));
    }

    public override RoutableCommandOptionArgumentsType ArgumentsType => RoutableCommandOptionArgumentsType.Many;

    public override void Parse(string argument)
    {
        _values.Add(argument);
    }

    public override void EndOfArguments()
    {
        if (_values.Count == 0)
            throw new InvalidOperationException($"Option {MainName} needs at least one value");
    }

    public override void Inject(object target)
    {
        if (_values.Count > 0)
            _injectValue(target, _values.ToArray());
    }
}