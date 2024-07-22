using LVK.Core.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.ObjectDumper;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new Core.ModuleBootstrapper());
        bootstrapper.Bootstrap(new Typed.ModuleBootstrapper());

        builder.Services.AddSingleton<IObjectDumper>(_ => ObjectDumper.Instance);
    }
}