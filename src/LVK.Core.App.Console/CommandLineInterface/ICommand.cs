namespace LVK.Core.App.Console.CommandLineInterface;

public interface ICommand
{
    Task<int> RunAsync(CancellationToken cancellationToken);
}