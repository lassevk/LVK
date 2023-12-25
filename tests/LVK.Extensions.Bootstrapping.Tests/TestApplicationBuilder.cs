using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;

namespace LVK.Extensions.Bootstrapping.Tests;

public class TestApplicationBuilder : IHostApplicationBuilder
{
    public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null)
        where TContainerBuilder : notnull { }

    public IDictionary<object, object> Properties { get; } = null!;
    public IConfigurationManager Configuration { get; } = null!;
    public IHostEnvironment Environment { get; } = null!;
    public ILoggingBuilder Logging { get; } = null!;
    public IMetricsBuilder Metrics { get; } = null!;
    public IServiceCollection Services { get; } = new ServiceCollection();

    public TestHost Build()
    {
        IServiceProvider serviceProvider = new TestServiceProvider(Services);
        return new TestHost(serviceProvider);
    }
}