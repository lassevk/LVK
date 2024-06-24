using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Hosting;

namespace LVK.Data.MongoDb;

public sealed class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new LVK.Typed.ModuleBootstrapper());
    }
}