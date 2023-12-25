using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Extensions.Bootstrapping.Console;

internal class ModuleBootstrapperWrapper : IModuleBootstrapper<HostApplicationBuilder, IHost>
{
    private readonly IModuleBootstrapper<HostApplicationBuilder, IHost> _moduleBootstrapper;

    public ModuleBootstrapperWrapper(IModuleBootstrapper<HostApplicationBuilder, IHost> moduleBootstrapper)
    {
        _moduleBootstrapper = moduleBootstrapper ?? throw new ArgumentNullException(nameof(moduleBootstrapper));
    }

    public void Bootstrap(IHostBootstrapper<HostApplicationBuilder, IHost> bootstrapper, HostApplicationBuilder builder)
    {
        _moduleBootstrapper.Bootstrap(bootstrapper, builder);

        builder.Services.AddHostedService<MainEntrypointRunner>();
    }
}