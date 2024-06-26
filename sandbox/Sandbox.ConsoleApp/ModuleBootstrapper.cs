using LVK.Core.App.Console;
using LVK.Core.Bootstrapping;
using LVK.Data.MongoDb;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MongoDB.Driver;

using Sandbox.ConsoleApp.Models;

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
           .Bootstrap(new LVK.Data.MongoDb.ModuleBootstrapper());

        builder.Services.AddMainEntrypoint<MainEntrypoint>();

        builder.Services.AddMongoClient(builder.Configuration.GetConnectionString("MongoDB")!);
        builder.Services.AddMongoDatabase(builder.Configuration["SSK:DatabaseName"]!);
        builder.Services.AddMongoCollection<RootModel>();
    }
}