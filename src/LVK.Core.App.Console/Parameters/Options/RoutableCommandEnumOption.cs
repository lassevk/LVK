namespace LVK.Core.App.Console.Parameters.Options;

internal class RoutableCommandEnumOption : RoutableCommandOption
{
    private readonly Type _enumType;
    private readonly Action<object, object?> _injectValue;
    private object? _value;

    public RoutableCommandEnumOption(string? longName, string? shortName, string? description, Type enumType, object? defaultValue, Action<object, object?> injectValue)
        : base(longName, shortName, description)
    {
        _value = defaultValue;
        _enumType = enumType;
        _injectValue = injectValue ?? throw new ArgumentNullException(nameof(injectValue));
    }

    public override RoutableCommandOptionArgumentsType ArgumentsType => RoutableCommandOptionArgumentsType.One;

    public override void Parse(string argument)
    {
        if (Enum.TryParse(_enumType, argument, true, out object? value))
            _value = value;
        else
            throw new InvalidOperationException($"Enum option with type {_enumType} does not support value {argument}");
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

    protected override IEnumerable<string> GetValueHelpText()
    {
        yield return "legal values: " + string.Join(", ", Enum.GetNames(_enumType));

        if (_value != null)
            yield return "  default: " + Enum.GetName(_enumType, _value);
    }
}