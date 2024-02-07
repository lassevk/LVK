using LVK.Core.App.Console;
using LVK.Core.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Sandbox.ConsoleApp.Services;

namespace Sandbox.ConsoleApp;

public class ModuleBootstrapper : IApplicationBootstrapper<HostApplicationBuilder, IHost>
{
    public void Bootstrap(IHostBootstrapper<HostApplicationBuilder, IHost> bootstrapper, HostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new LVK.Events.ModuleBootstrapper())
           .Bootstrap(new LVK.Extensions.Logging.SmartInspect.ModuleBootstrapper())
           .Bootstrap(new LVK.Events.ModuleBootstrapper())
           .Bootstrap(new LVK.ObjectDumper.ModuleBootstrapper())
           .Bootstrap(new LVK.Events.ModuleBootstrapper())
           .Bootstrap(new LVK.Notifications.Pushover.ModuleBootstrapper());

        builder.Services.AddTransient<IService, Service>();
        builder.Services.AddMainEntrypoint<MainEntrypoint>();
    }
}