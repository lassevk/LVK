using Gurock.SmartInspect;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace LVK.Extensions.Logging.SmartInspect;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddSmartInspectLogging(this ILoggingBuilder builder)
    {
        builder.Services.AddSingleton(SiAuto.Main);
        builder.Services.AddSingleton(SiAuto.Si);

        builder.Services.AddHostedService<SmartInspectProcessLogger>();
        builder.Services.AddHostedService<SmartInspectBackgroundMonitors>();
        builder.Services.AddSingleton<ILoggerProvider, SmartInspectLoggerProvider>();
        LoggerProviderOptions.RegisterProviderOptions<SmartInspectLoggerConfiguration, SmartInspectLoggerProvider>(builder.Services);

        return builder;
    }

    public static ILoggingBuilder AddSmartInspectLogging(this ILoggingBuilder builder,
            Action<SmartInspectLoggerConfiguration> configure)
    {
        builder.AddSmartInspectLogging();
        builder.Services.Configure(configure);

        return builder;
    }
}