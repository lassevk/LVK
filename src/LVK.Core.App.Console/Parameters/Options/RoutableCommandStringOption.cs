namespace LVK.Core.App.Console.Parameters.Options;

internal class RoutableCommandStringOption : RoutableCommandOption
{
    private readonly Action<object, string?> _injectValue;
    private string? _value;

    public RoutableCommandStringOption(string? longName, string? shortName, string? description, string? defaultValue, Action<object, string?> injectValue)
        : base(longName, shortName, description)
    {
        _value = defaultValue;
        _injectValue = injectValue ?? throw new ArgumentNullException(nameof(injectValue));
    }

    public override RoutableCommandOptionArgumentsType ArgumentsType => RoutableCommandOptionArgumentsType.One;

    public override void Parse(string argument)
    {
        _value = argument;
    }

    public override void EndOfArguments()
    {
        if (_value == null)
            throw new InvalidOperationException($"Option {MainName} needs an argument");
    }

    public override void Inject(object target)
    {
        if (_value != null)
            _injectValue(target, _value);
    }
}