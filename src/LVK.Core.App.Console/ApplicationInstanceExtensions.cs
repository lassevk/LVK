using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Hosting;

namespace LVK.Core.App.Console;

public static class ApplicationInstanceExtensions
{
    public static Task RunAsConsole(this IApplication application, string[] args, params IApplicationBootstrapper<HostApplicationBuilder, IHost>[] applicationBootstrappers)
    {
        Guard.NotNull(application);
        Guard.NotNull(args);
        Guard.NotNull(applicationBootstrappers);

        return application.BootstrapAndBuild(Host.CreateApplicationBuilder(args), b => b.Build(), [..applicationBootstrappers, new ConsoleApplicationBootstrapper()]).RunAsync();
    }

    public static Task RunAsConsole(this IApplication application, params IApplicationBootstrapper<HostApplicationBuilder, IHost>[] applicationBootstrappers)
    {
        Guard.NotNull(application);
        Guard.NotNull(applicationBootstrappers);

        return application.BootstrapAndBuild(Host.CreateApplicationBuilder(), b => b.Build(), [..applicationBootstrappers, new ConsoleApplicationBootstrapper()]).RunAsync();
    }
}