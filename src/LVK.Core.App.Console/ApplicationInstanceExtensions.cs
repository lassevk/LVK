using LVK.Core.App.Console.CommandLineInterface;
using LVK.Core.App.Console.ConsoleApplication;
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

        HostApplicationBuilder hostApplicationBuilder = Host.CreateApplicationBuilder(args);
        hostApplicationBuilder.Services.AddSystemd();

        return application.BootstrapAndBuild(hostApplicationBuilder, b => b.Build(), [..applicationBootstrappers, new ConsoleApplicationBootstrapper()]).RunAsync();
    }

    public static Task RunAsConsole(this IApplication application, params IApplicationBootstrapper<HostApplicationBuilder, IHost>[] applicationBootstrappers)
    {
        Guard.NotNull(application);
        Guard.NotNull(applicationBootstrappers);

        HostApplicationBuilder hostApplicationBuilder = Host.CreateApplicationBuilder();
        hostApplicationBuilder.Services.AddSystemd();

        return application.BootstrapAndBuild(hostApplicationBuilder, b => b.Build(), [..applicationBootstrappers, new ConsoleApplicationBootstrapper()]).RunAsync();
    }

    public static Task RunAsCommandLineInterface(this IApplication application, string[] args, params IApplicationBootstrapper<HostApplicationBuilder, IHost>[] applicationBootstrappers)
    {
        Guard.NotNull(application);
        Guard.NotNull(applicationBootstrappers);
        Guard.NotNull(args);

        return application.BootstrapAndBuild(Host.CreateApplicationBuilder(args), b => b.Build(), [..applicationBootstrappers, new CommandLineInterfaceBootstrapper()]).RunAsync();
    }
}