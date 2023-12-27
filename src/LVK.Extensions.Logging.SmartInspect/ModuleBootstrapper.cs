using Gurock.SmartInspect;

using LVK.Core.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace LVK.Extensions.Logging.SmartInspect;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(SiAuto.Main);
        builder.Services.AddSingleton(SiAuto.Si);

        builder.Services.AddHostedService<SmartInspectProcessLogger>();
        builder.Services.AddHostedService<SmartInspectBackgroundMonitors>();
        builder.Services.AddSingleton<ILoggerProvider, SmartInspectLoggerProvider>();
        LoggerProviderOptions.RegisterProviderOptions<SmartInspectLoggerConfiguration, SmartInspectLoggerProvider>(builder.Services);
    }
}