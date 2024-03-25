using LVK.Core.App.Console;
using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Hosting;

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
           .Bootstrap(new LVK.Notifications.Pushover.ModuleBootstrapper())
           .Bootstrap(new LVK.Data.EntityFramework.ModuleBootstrapper())
           .Bootstrap(new LVK.Data.EntityFramework.Sqlite.ModuleBootstrapper())
           .Bootstrap(new LVK.Data.EntityFramework.MySql.ModuleBootstrapper())
           .Bootstrap(new LVK.Data.BlobStorage.ModuleBootstrapper())
           .Bootstrap(new LVK.Security.OnePassword.ModuleBootstrapper());

        builder.Services.AddMainEntrypoint<MainEntrypoint>();
    }
}