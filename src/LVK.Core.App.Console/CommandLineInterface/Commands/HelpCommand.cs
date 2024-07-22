using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

using LVK.Core.App.Console.CommandLineInterface.RoutableCommandsProviders;
using LVK.Core.App.Console.Parameters;

namespace LVK.Core.App.Console.CommandLineInterface.Commands;

[Description("Provides help for application and individual commands")]
internal class HelpCommand : ICommand
{
    private readonly IEnumerable<IRoutableCommandsProvider> _routableCommandsProviders;

    public HelpCommand(IEnumerable<IRoutableCommandsProvider> routableCommandsProviders)
    {
        _routableCommandsProviders = routableCommandsProviders ?? throw new ArgumentNullException(nameof(routableCommandsProviders));
    }

    [PositionalArguments]
    public List<string> SpecificCommands { get; } = new();

    public async Task<int> RunAsync(CancellationToken cancellationToken)
    {
        // TODO: Help for one command

        await ShowHelpForAllCommands(cancellationToken);

        return 0;
    }

    private async Task ShowHelpForAllCommands(CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetEntryAssembly();

        var commands = new List<IRoutableCommand>();
        foreach (IRoutableCommandsProvider provider in _routableCommandsProviders)
            commands.AddRange(await provider.GetCommandsAsync(cancellationToken));

        commands.Sort((c1, c2) => StringComparer.InvariantCultureIgnoreCase.Compare(c1.Name, c2.Name));

        System.Console.WriteLine($"{assembly?.GetName().Name ?? GetProcessName()} (command) [arguments] [-options] [--options]");

        System.Console.WriteLine("commands:");
        foreach (IRoutableCommand command in commands)
        {
            foreach (string line in command.GetHelpText())
                System.Console.WriteLine("  " + line);
        }
    }

    private string GetProcessName()
    {
        using var process = Process.GetCurrentProcess();
        return process.ProcessName;
    }
}