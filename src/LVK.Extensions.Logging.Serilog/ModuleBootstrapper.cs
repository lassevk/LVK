using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace LVK.Extensions.Logging.Serilog;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        var serilogConfiguration = new SerilogConfiguration();
        builder.Configuration.GetSection("Logging:Serilog").Bind(serilogConfiguration);
        if (!serilogConfiguration.IsValidAndEnabled())
            return;

        var loggerConfiguration = new LoggerConfiguration();
        loggerConfiguration = (serilogConfiguration.MinLevel ?? SerilogLevel.Debug) switch
        {
            SerilogLevel.Debug       => loggerConfiguration.MinimumLevel.Debug(),
            SerilogLevel.Verbose     => loggerConfiguration.MinimumLevel.Verbose(),
            SerilogLevel.Information => loggerConfiguration.MinimumLevel.Information(),
            SerilogLevel.Warning     => loggerConfiguration.MinimumLevel.Warning(),
            SerilogLevel.Error       => loggerConfiguration.MinimumLevel.Error(),
            SerilogLevel.Fatal       => loggerConfiguration.MinimumLevel.Fatal(),
            _                        => throw new ArgumentOutOfRangeException(),
        };

        loggerConfiguration = loggerConfiguration.WriteTo.File(serilogConfiguration.Path!, rollingInterval: serilogConfiguration.RollingInterval ?? RollingInterval.Day,
            fileSizeLimitBytes: (serilogConfiguration.FileSizeLimit ?? 256 * 1024) * 1024, rollOnFileSizeLimit: true, retainedFileCountLimit: 90,
            retainedFileTimeLimit: TimeSpan.FromDays(serilogConfiguration.RetentionDays ?? 90));

        Log.Logger = loggerConfiguration.CreateLogger();

        builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
    }
}