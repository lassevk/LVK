using LVK.Core.Bootstrapping;
using LVK.ObjectDumper.Rules;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.ObjectDumper;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new LVK.Core.ModuleBootstrapper());
        bootstrapper.Bootstrap(new LVK.Typed.ModuleBootstrapper());

        builder.Services.AddSingleton<IObjectDumper>(_ => ObjectDumper.Instance);
    }
}