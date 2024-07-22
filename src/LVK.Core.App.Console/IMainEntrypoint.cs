namespace LVK.Core.App.Console;

public interface IMainEntrypoint
{
    Task<int> RunAsync(CancellationToken stoppingToken);
}