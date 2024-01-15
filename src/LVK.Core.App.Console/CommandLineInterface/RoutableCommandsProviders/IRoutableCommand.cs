namespace LVK.Core.App.Console.CommandLineInterface.RoutableCommandsProviders;

internal interface IRoutableCommand
{
    string Name { get; }
    IEnumerable<string> GetHelpText();

    Task<int> InvokeAsync(IServiceProvider serviceProvider, string[] arguments, CancellationToken cancellationToken);
}