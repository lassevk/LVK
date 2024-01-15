namespace LVK.Core.App.Console.CommandLineInterface.RoutableCommandsProviders;

internal interface IRoutableCommandsProvider
{
    Task<List<IRoutableCommand>> GetCommandsAsync(CancellationToken cancellationToken);
}