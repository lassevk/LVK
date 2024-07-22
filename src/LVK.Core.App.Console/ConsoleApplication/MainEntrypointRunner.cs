using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

using LVK.Core.App.Console.Parameters;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LVK.Core.App.Console.ConsoleApplication;

internal class MainEntrypointRunner : IHostedService
{
    private readonly IHostApplicationLifetime _hostLifetime;
    private readonly ILogger<MainEntrypointRunner> _logger;
    private readonly IMainEntrypoint? _mainEntrypoint;
    private Task<int>? _task;

    public MainEntrypointRunner(IHostApplicationLifetime hostApplicationLifetime, ILogger<MainEntrypointRunner> logger, IMainEntrypoint? mainEntrypoint = null)
    {
        _mainEntrypoint = mainEntrypoint;
        _hostLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_mainEntrypoint != null)
            _task = RunMainEntryPoint(cancellationToken);

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_task != null)
            Environment.ExitCode = await _task;
    }

    private async Task<int> RunMainEntryPoint(CancellationToken cancellationToken)
    {
        try
        {
            if (_mainEntrypoint == null)
            {
                await System.Console.Error.WriteLineAsync("No main entry point registered, unable to continue");
                return 1;
            }

            var cts = new CancellationTokenSource();
            cancellationToken.Register(() => cts.Cancel());
            _hostLifetime.ApplicationStopping.Register(() => cts.Cancel());

            string[] arguments = Environment.GetCommandLineArgs().Skip(1).ToArray();
            if (arguments.Any(a => StringComparer.InvariantCultureIgnoreCase.Equals("-h", a) || StringComparer.InvariantCultureIgnoreCase.Equals("--help", a)))
            {
                _logger.LogDebug("Providing help for application instead of executing it");
                ShowHelp();
                return 0;
            }

            InstanceParameterHandler.InjectParameters(_mainEntrypoint, arguments);

            int exitCode = await _mainEntrypoint!.RunAsync(cts.Token);
            _logger.LogInformation("Application code terminated gracefully, exiting with exit code {ExitCode}", Environment.ExitCode);
            return exitCode;
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
            _hostLifetime.StopApplication();
        }
    }

    private void ShowHelp()
    {
        if (_mainEntrypoint == null)
        {
            System.Console.Error.WriteLine("No entry point, unable to provide help");
            return;
        }

        DescriptionAttribute? attr = _mainEntrypoint!.GetType().GetCustomAttribute<DescriptionAttribute>();
        string description = attr?.Description ?? "No description provided";

        System.Console.WriteLine($"{GetApplicationName()}: {description}");
        foreach (string line in InstanceParameterHandler.ProvideParameterHelp(_mainEntrypoint.GetType()))
            System.Console.WriteLine("  " + line);
    }

    private string GetApplicationName()
    {
        var assembly = Assembly.GetEntryAssembly();
        string? name = assembly?.GetName().Name;
        if (name != null)
            return name;

        using var process = Process.GetCurrentProcess();
        return process.ProcessName;
    }
}