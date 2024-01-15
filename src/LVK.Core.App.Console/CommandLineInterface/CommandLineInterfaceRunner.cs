using System.Diagnostics;

using LVK.Core.App.Console.CommandLineInterface.RoutableCommandsProviders;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LVK.Core.App.Console.CommandLineInterface;

internal class CommandLineInterfaceRunner : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<CommandLineInterfaceRunner> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEnumerable<IRoutableCommandsProvider> _routableCommandsProviders;
    private readonly IHostEnvironment _hostEnvironment;
    private Task<int>? _task;

    public CommandLineInterfaceRunner(
        IHostApplicationLifetime hostApplicationLifetime, ILogger<CommandLineInterfaceRunner> logger, IServiceProvider serviceProvider,
        IEnumerable<IRoutableCommandsProvider> routableCommandsProviders, IHostEnvironment hostEnvironment)
    {
        _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _routableCommandsProviders = routableCommandsProviders ?? throw new ArgumentNullException(nameof(routableCommandsProviders));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _task = RunCommands(cancellationToken);

        return Task.CompletedTask;
    }

    private async Task<int> RunCommands(CancellationToken cancellationToken)
    {
        try
        {
            var cts = new CancellationTokenSource();
            cancellationToken.Register(() => cts.Cancel());
            _hostApplicationLifetime.ApplicationStopping.Register(() => cts.Cancel());

            return await InvokeCommand(cts.Token);
        }
        catch (TaskCanceledException) when (!Debugger.IsAttached)
        {
            _logger.LogWarning("Application was forced to terminate");
            return 1;
        }
        catch (Exception ex) when (!Debugger.IsAttached)
        {
            _logger.LogError(ex, "Application terminated due to an exception, {Type}: {Message}", ex.GetType().Name, ex.Message);
            return 1;
        }
        finally
        {
            _hostApplicationLifetime.StopApplication();
        }
    }

    private async Task<int> InvokeCommand(CancellationToken cancellationToken)
    {
        var arguments = Environment.GetCommandLineArgs().Skip(1).ToList();
        string commandName = GetCommandName(arguments) ?? "help";
        _logger.LogDebug("Resolved command line interface to command {CommandName}", commandName);

        foreach (IRoutableCommandsProvider provider in _routableCommandsProviders)
        {
            foreach (IRoutableCommand command in await provider.GetCommandsAsync(cancellationToken))
            {
                if (StringComparer.InvariantCultureIgnoreCase.Equals(commandName, command.Name))
                {
                    _logger.LogDebug("Routing command line interface for {CommandName}", commandName);
                    return await command.InvokeAsync(_serviceProvider, arguments.ToArray(), cancellationToken);
                }
            }
        }

        _logger.LogDebug("No command line interface command registered for {CommandName}", commandName);
        await System.Console.Error.WriteLineAsync($"No such command, '{commandName}', invoke the 'help' command to get a list of supported commands");

        return 1;
    }

    private string? GetCommandName(List<string> arguments)
    {
        for (var index = 0; index < arguments.Count; index++)
        {
            if (arguments[index].StartsWith("-"))
                continue;

            string commandName = arguments[index];
            arguments.RemoveAt(index);
            return commandName;
        }

        return null;
    }

    private Task<int> ShowHelp()
    {
        System.Console.WriteLine("help");
        return Task.FromResult(1);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_task != null)
            Environment.ExitCode = await _task;
    }
}