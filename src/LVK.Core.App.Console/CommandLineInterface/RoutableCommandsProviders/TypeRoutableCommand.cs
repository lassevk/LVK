using System.ComponentModel;
using System.Reflection;

using LVK.Core.App.Console.Parameters;

using Microsoft.Extensions.DependencyInjection;

namespace LVK.Core.App.Console.CommandLineInterface.RoutableCommandsProviders;

internal class TypeRoutableCommand : IRoutableCommand
{
    private readonly Lazy<string> _commandName;
    private readonly Lazy<string> _description;
    private readonly Type _type;

    public TypeRoutableCommand(Type type)
    {
        _type = type ?? throw new ArgumentNullException(nameof(type));
        _commandName = new Lazy<string>(GetCommandName);
        _description = new Lazy<string>(GetDescription);
    }

    public string Name => _commandName.Value;

    public IEnumerable<string> GetHelpText()
    {
        yield return $"{Name.ToLowerInvariant()}: {_description.Value}";

        foreach (string line in InstanceParameterHandler.ProvideParameterHelp(_type))
            yield return "  " + line;
    }

    public async Task<int> InvokeAsync(IServiceProvider serviceProvider, string[] arguments, CancellationToken cancellationToken)
    {
        var command = serviceProvider.GetRequiredService(_type) as ICommand;
        if (command is null)
            throw new InvalidOperationException("Internal exception, command type does not inherit from CommandBase");

        InstanceParameterHandler.InjectParameters(command, arguments);

        return await command.RunAsync(cancellationToken);
    }

    private string GetCommandName()
    {
        CommandNameAttribute? attribute = _type.GetCustomAttribute<CommandNameAttribute>();
        string? commandName = attribute?.Name;
        if (!string.IsNullOrWhiteSpace(commandName))
            return commandName;

        commandName = _type.Name;
        if (commandName.EndsWith("Command"))
            commandName = commandName[..^7];

        return commandName;
    }

    private string GetDescription()
    {
        DescriptionAttribute? attribute = _type.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? "No description provided";
    }
}