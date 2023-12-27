using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Hosting;

namespace LVK.Core.Tests.Bootstrapping;

public class CallBootstrapTwiceBootstrapper : IApplicationBootstrapper<HostApplicationBuilder, IHost>
{
    private readonly IApplicationBootstrapper<HostApplicationBuilder, IHost> _bootstrapper;

    public CallBootstrapTwiceBootstrapper(IApplicationBootstrapper<HostApplicationBuilder, IHost> bootstrapper)
    {
        _bootstrapper = bootstrapper;
    }

    public void Bootstrap(IHostBootstrapper<HostApplicationBuilder, IHost> bootstrapper, HostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(_bootstrapper);
        bootstrapper.Bootstrap(_bootstrapper);
    }
}