using LVK.Extensions.Bootstrapping;

using Microsoft.Extensions.Hosting;

namespace LVK.Extensions.Logging.SmartInspect.Bootstrapped;

public class ModuleBootstrapper : IModuleBootstrapper
{
    /// <inheritdoc cref="IModuleBootstrapper.Bootstrap"/>
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        builder.Logging.AddSmartInspectLogging();
    }
}