namespace LVK.Extensions.Bootstrapping.Console;

/// <summary>
/// Implement this interface on a type and register it in the service collection to make the console
/// application run this code, and then terminate.
/// </summary>
public interface IMainEntrypoint
{
    /// <summary>
    /// This method will be called, and when it returns or throws an exception, the application will terminate.
    /// </summary>
    /// <param name="stoppingToken">
    /// A <see cref="CancellationToken"/> that can be used to detect forced shutdown of the application.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> containing the exit code of the application.
    /// </returns>
    Task<int> RunAsync(CancellationToken stoppingToken);
}