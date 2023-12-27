using LVK.Core.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Core.Tests.Bootstrapping;

public class TestServiceProvider : IServiceProvider
{
    private readonly IServiceCollection _services;

    public TestServiceProvider(IServiceCollection services)
    {
        _services = services;
    }

    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IEnumerable<IModuleInitializer<IHost>>))
            return _services.Select(sd => sd.ImplementationInstance).OfType<IModuleInitializer<IHost>>().ToList();

        if (serviceType == typeof(IEnumerable<IModuleInitializer<TestHost>>))
            return _services.Select(sd => sd.ImplementationInstance).OfType<IModuleInitializer<TestHost>>().ToList();

        return _services.FirstOrDefault(sd => sd.ServiceType == serviceType)?.ImplementationInstance;
    }
}