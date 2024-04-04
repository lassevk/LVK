using System.Runtime.CompilerServices;

using LVK.Core.App.Console;
using LVK.Core.Bootstrapping;
using LVK.Data.Processing;

using Microsoft.Extensions.DependencyInjection;
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
           .Bootstrap(new LVK.Security.OnePassword.ModuleBootstrapper())
           .Bootstrap(new LVK.Data.Processing.ModuleBootstrapper());

        builder.Services.AddMainEntrypoint<MainEntrypoint>();

        builder.Services.AddSingleton<IEntityProcessorProvider, MyEntityProcessorProvider>();
    }
}

public class MyEntityProcessorProvider : IEntityProcessorProvider
{
    public async IAsyncEnumerable<IEntityProcessor> ProvideAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.Yield();
        yield return EntityProcessor.Create((InputComponent cmp) => new OutputComponent(cmp.Value.ToString()));

        yield return new EntityProcessorBuilder().Add((OutputComponent cmp) => new OutputLengthComponent(cmp.Value.Length)).Build();
    }
}