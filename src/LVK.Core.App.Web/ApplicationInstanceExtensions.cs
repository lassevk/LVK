using LVK.Core.Bootstrapping;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace LVK.Core.App.Web;

public static class ApplicationInstanceExtensions
{
    public static Task RunAsWebApplication(this IApplication application, string[] args, IApplicationBootstrapper<WebApplicationBuilder, WebApplication> applicationBootstrapper)
    {
        Guard.NotNull(application);
        Guard.NotNull(args);
        Guard.NotNull(applicationBootstrapper);

        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
        webApplicationBuilder.Services.AddSystemd();

        return application.BootstrapAndBuild(webApplicationBuilder, b => b.Build(), applicationBootstrapper, new WebApplicationBootstrapper()).RunAsync();
    }
}