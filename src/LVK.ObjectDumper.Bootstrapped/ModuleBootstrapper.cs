using LVK.Extensions.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.ObjectDumper.Bootstrapped;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new LVK.Typed.Bootstrapped.ModuleBootstrapper());

        builder.Services.AddSingleton<IObjectDumper, ObjectDumper>();
        foreach (IObjectDumperRule rule in ObjectDumperFactory.GetRules())
            builder.Services.AddSingleton(rule);
    }
}