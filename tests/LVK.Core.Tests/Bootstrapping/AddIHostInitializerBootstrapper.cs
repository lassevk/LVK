using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Hosting;

namespace LVK.Core.Tests.Bootstrapping;

public class AddIHostInitializerBootstrapper : IModuleBootstrapper, IModuleInitializer<IHost>
{
    private readonly IModuleInitializer<IHost> _initializer;

    public AddIHostInitializerBootstrapper(IModuleInitializer<IHost> initializer)
    {
        _initializer = initializer;
    }

    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
    }

    public void Initialize(IHost host)
    {
        _initializer.Initialize(host);
    }
}