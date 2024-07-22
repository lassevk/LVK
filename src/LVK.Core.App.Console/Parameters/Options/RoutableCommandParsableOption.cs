using System.Reflection;

namespace LVK.Core.App.Console.Parameters.Options;

internal class RoutableCommandParsableOption : RoutableCommandOption
{
    private readonly Action<object, object?> _injectValue;
    private readonly Type _targetType;
    private object? _value;

    public RoutableCommandParsableOption(string? longName, string? shortName, string? description, Type targetType, object? defaultValue, Action<object, object?> injectValue)
        : base(longName, shortName, description)
    {
        _targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        _value = defaultValue;
        _injectValue = injectValue ?? throw new ArgumentNullException(nameof(injectValue));
    }

    public override RoutableCommandOptionArgumentsType ArgumentsType => RoutableCommandOptionArgumentsType.One;

    public override void Parse(string argument)
    {
        if (argument == "")
        {
            _value = Activator.CreateInstance(_targetType);
            return;
        }

        MethodInfo? parse = _targetType.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(c
            => c.Name == "Parse" && c.GetParameters().Length == 2 && c.GetParameters()[0].ParameterType == typeof(string) && c.GetParameters()[1].ParameterType == typeof(IFormatProvider));

        _value = parse?.Invoke(null, [argument, null]);
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