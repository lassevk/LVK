using System.ComponentModel;
using System.Reflection;

using LVK.Core.App.Console.CommandLineInterface;
using LVK.Core.App.Console.Parameters.Options;
using LVK.Core.Extensions;

namespace LVK.Core.App.Console.Parameters;

internal static class InstanceParameterHandler
{
    // ReSharper disable once CognitiveComplexity
    public static void InjectParameters(object instance, string[] arguments)
    {
        Guard.NotNull(instance);
        Guard.NotNull(arguments);

        if (arguments.Length == 0)
            return;

        var options = GetOptions(instance.GetType()).ToList();
        List<string>? positionalArgumentsCollection = null;
        var index = 0;
        int last = -1;
        RoutableCommandOption? currentOption = null;
        while (index < arguments.Length)
        {
            if (index == -last)
                throw new InvalidOperationException("Internal error, unable to continue parsing options for command");

            last = index;

            string argument = arguments[index];
            if (argument.StartsWith("-"))
            {
                currentOption?.EndOfArguments();

                if (argument.StartsWith("--"))
                {
                    string optionName = argument[2..];
                    currentOption = options.FirstOrDefault(o => StringComparer.InvariantCultureIgnoreCase.Equals(optionName, o.LongName));
                }
                else
                {
                    string optionName = argument[1..];
                    currentOption = options.FirstOrDefault(o => StringComparer.InvariantCultureIgnoreCase.Equals(optionName, o.ShortName));
                }

                if (currentOption == null)
                    throw new InvalidOperationException($"No option with the name {argument}");

                if (currentOption.ArgumentsType == RoutableCommandOptionArgumentsType.None)
                {
                    currentOption.EndOfArguments();
                    currentOption = null;
                }
            }
            else
            {
                if (currentOption == null)
                {
                    if (positionalArgumentsCollection == null)
                    {
                        PropertyInfo? positionalArgumentsProperty = GetPositionalArgumentsProperty(instance.GetType());
                        if (positionalArgumentsProperty == null)
                            throw new InvalidOperationException("This does not accept positional arguments");

                        positionalArgumentsCollection = positionalArgumentsProperty.GetValue(instance) as List<string>;
                        if (positionalArgumentsCollection == null)
                            throw new InvalidOperationException("The positional arguments property must contain an already created instance of List<string>");
                    }

                    positionalArgumentsCollection.Add(argument);
                }
                else
                {
                    currentOption.Parse(argument);
                    if (currentOption.ArgumentsType == RoutableCommandOptionArgumentsType.One)
                    {
                        currentOption.EndOfArguments();
                        currentOption = null;
                    }
                }
            }

            index++;
        }

        currentOption?.EndOfArguments();
        foreach (RoutableCommandOption option in options)
            option.Inject(instance);
    }

    public static IEnumerable<string> ProvideParameterHelp(Type instanceType)
    {
        var options = GetOptions(instanceType).ToList();

        if (GetPositionalArgumentsProperty(instanceType) != null)
            yield return "Accepts positional arguments";

        if (options.Any())
        {
            yield return "";
            yield return "options:";
            foreach (RoutableCommandOption option in options)
            {
                foreach (string line in option.GetHelpText())
                    yield return "  " + line;
            }
        }
    }

    // ReSharper disable once CognitiveComplexity
    private static IEnumerable<RoutableCommandOption> GetOptions(Type type)
    {
        foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var attributes = property.GetCustomAttributes<CommandLineOption>().ToList();
            if (attributes.Count == 0)
                continue;

            string? shortName = null;
            string? longName = null;
            foreach (CommandLineOption attribute in attributes)
            {
                if (attribute.Name.Length == 1)
                    shortName = attribute.Name;
                else
                    longName = attribute.Name;
            }

            DescriptionAttribute? descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();
            string? description = descriptionAttribute?.Description;

            if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            {
                DefaultValueAttribute? defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
                var defaultValue = defaultValueAttribute?.Value as bool?;
                yield return new RoutableCommandBooleanOption(longName, shortName, description, defaultValue, (command, value) => property.SetValue(command, value));
            }
            else if (property.PropertyType == typeof(string))
            {
                DefaultValueAttribute? defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
                var defaultValue = defaultValueAttribute?.Value as string;
                yield return new RoutableCommandStringOption(longName, shortName, description, defaultValue, (command, value) => property.SetValue(command, value));
            }
            else if (typeof(ICollection<string>).IsAssignableFrom(property.PropertyType))
            {
                yield return new RoutableCommandStringsOption(longName, shortName, description, (command, values) =>
                {
                    var collection = property.GetValue(command) as ICollection<string>;
                    if (collection == null)
                    {
                        object? value = Activator.CreateInstance(property.PropertyType);
                        property.SetValue(command, value);
                        collection = value as ICollection<string>;
                    }

                    collection!.AddRange(values);
                });
            }
            else if (property.PropertyType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IParsable<>)))
            {
                DefaultValueAttribute? defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
                object? defaultValue = defaultValueAttribute?.Value;

                yield return new RoutableCommandParsableOption(longName, shortName, description, property.PropertyType, defaultValue, (command, value) => property.SetValue(command, value));
            }
            else if (property.PropertyType.IsEnum)
            {
                DefaultValueAttribute? defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
                object? defaultValue = defaultValueAttribute?.Value;

                yield return new RoutableCommandEnumOption(longName, shortName, description, property.PropertyType, defaultValue, (command, value) => property.SetValue(command, value));
            }
            else
                throw new InvalidOperationException($"Option property has type {property.PropertyType} which is not supported");
        }
    }

    private static PropertyInfo? GetPositionalArgumentsProperty(Type type) => type
       .GetProperties(BindingFlags.Instance | BindingFlags.Public)
       .Where(property => property.PropertyType == typeof(List<string>))
       .FirstOrDefault(property => property.GetCustomAttribute<PositionalArgumentsAttribute>() != null);
}