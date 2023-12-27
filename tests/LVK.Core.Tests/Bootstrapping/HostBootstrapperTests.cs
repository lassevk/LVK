using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Hosting;

using NSubstitute;

namespace LVK.Core.Tests.Bootstrapping;

public class HostBootstrapperTests
{
    [Test]
    public void Bootstrap_CalledTwiceForSameGenericModuleBootstrapper_OnlyBootstrapsModuleOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IHostBootstrapper<IHostApplicationBuilder, IHost> hostBootstrapper = new HostBootstrapper<IHostApplicationBuilder, IHost>(builder);

        IApplicationBootstrapper<IHostApplicationBuilder, IHost>? moduleBootstrapper = Substitute.For<IApplicationBootstrapper<IHostApplicationBuilder, IHost>>();
        hostBootstrapper.Bootstrap(moduleBootstrapper);
        hostBootstrapper.Bootstrap(moduleBootstrapper);

        moduleBootstrapper.Received(1).Bootstrap(Arg.Any<IHostBootstrapper<IHostApplicationBuilder, IHost>>(), Arg.Any<IHostApplicationBuilder>());
    }

    [Test]
    public void Bootstrap_CalledTwiceForSameNonGenericModuleBootstrapper_OnlyBootstrapsModuleOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        IHostBootstrapper hostBootstrapper = new HostBootstrapper<IHostApplicationBuilder, IHost>(builder);

        IModuleBootstrapper? moduleBootstrapper = Substitute.For<IModuleBootstrapper>();
        hostBootstrapper.Bootstrap(moduleBootstrapper);
        hostBootstrapper.Bootstrap(moduleBootstrapper);

        moduleBootstrapper.Received(1).Bootstrap(Arg.Any<IHostBootstrapper>(), Arg.Any<IHostApplicationBuilder>());
    }
}