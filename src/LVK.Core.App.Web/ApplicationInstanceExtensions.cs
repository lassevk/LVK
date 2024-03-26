using LVK.Core.Bootstrapping;

using Microsoft.AspNetCore.Builder;

namespace LVK.Core.App.Web;

public static class ApplicationInstanceExtensions
{
    public static Task RunAsWebApplication(this IApplication application, string[] args, IApplicationBootstrapper<WebApplicationBuilder, WebApplication> applicationBootstrapper)
    {
        Guard.NotNull(application);
        Guard.NotNull(args);
        Guard.NotNull(applicationBootstrapper);

        return application.BootstrapAndBuild(WebApplication.CreateBuilder(args), b => b.Build(), applicationBootstrapper, new WebApplicationBootstrapper()).RunAsync();
    }
}