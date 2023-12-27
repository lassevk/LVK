using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Hosting;

namespace LVK.Core.Tests.Bootstrapping;

public class RecursiveBootstrapperBugTests
{
    [Test]
    public void Bootstrap_WithRecursiveBootstrapper_ThrowsInvalidOperationException()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IHostBootstrapper<IHostApplicationBuilder, IHost> bootstrapper = new HostBootstrapper<IHostApplicationBuilder, IHost>(builder);

        Assert.Throws<InvalidOperationException>(() => bootstrapper.Bootstrap(new RecursiveBootstrapper()));
    }
}

public class RecursiveBootstrapper : IApplicationBootstrapper<IHostApplicationBuilder, IHost>
{
    public void Bootstrap(IHostBootstrapper<IHostApplicationBuilder, IHost> bootstrapper, IHostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new RecursiveBootstrapper());
    }
}