using System.Diagnostics;

using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Hosting;

namespace LVK.Core.App.WindowsService;

internal class WindowsServiceBootstrapper : IApplicationBootstrapper<HostApplicationBuilder, IHost>
{
    public void Bootstrap(IHostBootstrapper<HostApplicationBuilder, IHost> bootstrapper, HostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new LVK.Core.ModuleBootstrapper());

        builder.Services.AddWindowsService(options =>
        {
            using var process = Process.GetCurrentProcess();
            options.ServiceName = process.ProcessName;
        });
    }
}