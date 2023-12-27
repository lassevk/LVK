using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Hosting;

namespace LVK.Core.App.WindowsService;

public static class ApplicationInstanceExtensions
{
    public static Task RunAsWindowsService(this IApplication application, string[] args, params IApplicationBootstrapper<HostApplicationBuilder, IHost>[] applicationBootstrappers)
    {
        Guard.NotNull(application);
        Guard.NotNull(args);
        Guard.NotNull(applicationBootstrappers);

        return application.BootstrapAndBuild(Host.CreateApplicationBuilder(args), b => b.Build(), [..applicationBootstrappers, new WindowsServiceBootstrapper()]).RunAsync();
    }
}