using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Hosting;

using MongoDb.Bson.NodaTime;

namespace LVK.Data.MongoDb.NodaTime;

public sealed class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new LVK.NodaTime.ModuleBootstrapper());
        bootstrapper.Bootstrap(new MongoDb.ModuleBootstrapper());

        NodaTimeSerializers.Register();
    }
}