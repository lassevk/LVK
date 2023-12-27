using LVK.Core.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class ApplicationBootstrapper : IApplicationBootstrapper<HostApplicationBuilder, IHost>
{
    public void Bootstrap(IHostBootstrapper<HostApplicationBuilder, IHost> bootstrapper, HostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<PeriodicLogger>();
    }
}