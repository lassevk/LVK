using System.Diagnostics;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LVK.Core.App.Console;

internal class MainEntrypointRunner : IHostedService
{
    private readonly IMainEntrypoint? _mainEntrypoint;
    private readonly IHostApplicationLifetime _hostLifetime;
    private readonly ILogger<MainEntrypointRunner> _logger;
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

    private async Task<int> RunMainEntryPoint(CancellationToken cancellationToken)
    {
        try
        {
            var cts = new CancellationTokenSource();
            cancellationToken.Register(() => cts.Cancel());
            _hostLifetime.ApplicationStopping.Register(() => cts.Cancel());

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

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_task != null)
            Environment.ExitCode = await _task;
    }
}