namespace LVK.Extensions.Bootstrapping.Tests;

public class HostApplicationBuilderExtensionsTests
{
    [Test]
    public void Build_BootstrapCalledTwiceForSameModuleBootstrapper_OnlyBootstrapsModuleOnce()
    {
        IModuleBootstrapper<HostApplicationBuilder, IHost>? moduleBootstrapper = Substitute.For<IModuleBootstrapper<HostApplicationBuilder, IHost>>();
        _ = Host.CreateApplicationBuilder().BootstrapAndBuild(new CallBootstrapTwiceBootstrapper(moduleBootstrapper), b => b.Build());

        moduleBootstrapper.Received(1).Bootstrap(Arg.Any<IHostBootstrapper<HostApplicationBuilder, IHost>>(), Arg.Any<HostApplicationBuilder>());
    }

    [Test]
    public void Build_GenericBuilderWithRegisteredInitializer_InitializerIsCalled()
    {
        IModuleInitializer<TestHost> initializer = Substitute.For<IModuleInitializer<TestHost>>();
        _ = new TestApplicationBuilder().BootstrapAndBuild(new GenericBootstrapperWithInitializer(initializer), b => b.Build());

        initializer.Received(1).Initialize(Arg.Any<TestHost>());
    }

    [Test]
    public void Build_NonGenericBuilderWithRegisteredInitializer_InitializerIsCalled()
    {
        IModuleInitializer<IHost> initializer = Substitute.For<IModuleInitializer<IHost>>();
        _ = Host.CreateApplicationBuilder().BootstrapAndBuild(new NonGenericBootstrapperWithInitializer(initializer), b => b.Build());

        initializer.Received(1).Initialize(Arg.Any<IHost>());
    }
}