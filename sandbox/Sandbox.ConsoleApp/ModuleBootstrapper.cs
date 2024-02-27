using LVK.Core.App.Console;
using LVK.Core.Bootstrapping;
using LVK.Data.EntityFramework.MySql;
using LVK.Data.EntityFramework.Sqlite;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Sandbox.ConsoleApp.Services;

namespace Sandbox.ConsoleApp;

public class ModuleBootstrapper : IApplicationBootstrapper<HostApplicationBuilder, IHost>, IModuleInitializer<IHost>
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
           .Bootstrap(new LVK.Data.EntityFramework.MySql.ModuleBootstrapper());

        builder.Services.AddTransient<IService, Service>();
        builder.Services.AddMainEntrypoint<MainEntrypoint>();

        builder.AddMySqlDbContext<TestDbContext>("Default");
        // builder.AddSqliteDbContext<TestDbContext>("Default");
    }

    public void Initialize(IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        IDbContextFactory<TestDbContext> dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<TestDbContext>>();
        using TestDbContext dbContext = dbContextFactory.CreateDbContext();
        dbContext.Database.Migrate();
    }
}