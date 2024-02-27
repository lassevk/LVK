using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Hosting;

namespace LVK.Data.EntityFramework.Sqlite;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new LVK.Data.EntityFramework.ModuleBootstrapper());
    }
}