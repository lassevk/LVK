namespace LVK.Extensions.Bootstrapping.Tests;

public class NonGenericBootstrapperWithInitializer : IModuleBootstrapper<HostApplicationBuilder, IHost>
{
    private readonly IModuleInitializer<IHost> _initializer;

    public NonGenericBootstrapperWithInitializer(IModuleInitializer<IHost> initializer)
    {
        _initializer = initializer;
    }

    public void Bootstrap(IHostBootstrapper<HostApplicationBuilder, IHost> bootstrapper, HostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new NonGenericBootstrapper(_initializer));
    }
}