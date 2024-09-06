using System.Text;

using LVK.Core.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Core.App.Console.ConsoleApplication;

internal class ConsoleApplicationBootstrapper : IApplicationBootstrapper<HostApplicationBuilder, IHost>
{
    public void Bootstrap(IHostBootstrapper<HostApplicationBuilder, IHost> bootstrapper, HostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new ModuleBootstrapper());

        System.Console.OutputEncoding = Encoding.UTF8;

        builder.Services.AddHostedService<MainEntrypointRunner>();
    }
}