namespace LVK.Extensions.Bootstrapping.Tests;

public class GenericBootstrapperWithInitializer : IModuleBootstrapper<TestApplicationBuilder, TestHost>, IModuleInitializer<TestHost>
{
    private readonly IModuleInitializer<TestHost> _initializer;

    public GenericBootstrapperWithInitializer(IModuleInitializer<TestHost> initializer)
    {
        _initializer = initializer;
    }

    public void Bootstrap(IHostBootstrapper<TestApplicationBuilder, TestHost> bootstrapper, TestApplicationBuilder builder) { }

    public void Initialize(TestHost host)
    {
        _initializer.Initialize(host);
    }
}