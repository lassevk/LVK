using LVK.Extensions.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Typed.Bootstrapped;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ITypeHelper, TypeHelper>();
        foreach (ITypeNameRule rule in TypeHelperFactory.GetRules())
        {
            builder.Services.AddSingleton(rule);
        }
    }
}