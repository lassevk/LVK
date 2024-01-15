namespace LVK.Core.App.Console.Parameters.Options;

internal class RoutableCommandBooleanOption : RoutableCommandOption
{
    private readonly Action<object, bool> _injectValue;
    private bool? _value;

    public RoutableCommandBooleanOption(string? longName, string? shortName, string? description, bool? defaultValue, Action<object, bool> injectValue)
        : base(longName, shortName, description)
    {
        _injectValue = injectValue ?? throw new ArgumentNullException(nameof(injectValue));
        _value = defaultValue;
    }

    public override RoutableCommandOptionArgumentsType ArgumentsType => RoutableCommandOptionArgumentsType.One;

    public override void Parse(string argument)
    {
        switch (argument)
        {
            case "yes":
            case "true":
            case "on":
            case "1":
                _value = true;
                break;

            case "no":
            case "false":
            case "off":
            case "0":
                _value = false;
                break;

            default:
                throw new ArgumentOutOfRangeException($"Boolean option does not accept value {argument}");
        }
    }

    public override void EndOfArguments()
    {
        _value ??= true;
    }

    public override void Inject(object target)
    {
        if (_value != null)
            _injectValue(target, _value!.Value);
    }

    protected override IEnumerable<string> GetValueHelpText()
    {
        yield return "values that turn option on: 1, yes, on, true";
        yield return "values that turn option off: 0, no, off, false";
        yield return "value is mandatory, unless the option is the last argument, or it is followed by another option starting with '-'";
    }
}