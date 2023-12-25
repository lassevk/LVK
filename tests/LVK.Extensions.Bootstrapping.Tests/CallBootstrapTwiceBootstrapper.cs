namespace LVK.Extensions.Bootstrapping.Tests;

public class CallBootstrapTwiceBootstrapper : IModuleBootstrapper<HostApplicationBuilder, IHost>
{
    private readonly IModuleBootstrapper<HostApplicationBuilder, IHost> _bootstrapper;

    public CallBootstrapTwiceBootstrapper(IModuleBootstrapper<HostApplicationBuilder, IHost> bootstrapper)
    {
        _bootstrapper = bootstrapper;
    }

    public void Bootstrap(IHostBootstrapper<HostApplicationBuilder, IHost> bootstrapper, HostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(_bootstrapper);
        bootstrapper.Bootstrap(_bootstrapper);
    }
}