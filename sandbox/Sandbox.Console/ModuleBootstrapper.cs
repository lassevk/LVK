using LVK.Extensions.Bootstrapping;
using LVK.Extensions.Bootstrapping.Console;

using Microsoft.Extensions.Hosting;

namespace Sandbox.Console;

public class ModuleBootstrapper : IModuleBootstrapper<HostApplicationBuilder, IHost>
{
    public void Bootstrap(IHostBootstrapper<HostApplicationBuilder, IHost> bootstrapper, HostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new LVK.Events.Bootstrapped.ModuleBootstrapper()).Bootstrap(new LVK.Extensions.Logging.SmartInspect.Bootstrapped.ModuleBootstrapper())
           .Bootstrap(new LVK.Events.Bootstrapped.ModuleBootstrapper()).Bootstrap(new LVK.ObjectDumper.Bootstrapped.ModuleBootstrapper()).Bootstrap(new LVK.Events.Bootstrapped.ModuleBootstrapper());

        builder.Services.AddMainEntrypoint<MainEntrypoint>();
    }
}