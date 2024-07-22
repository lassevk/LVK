using LVK.Core.Bootstrapping;

using Microsoft.AspNetCore.Builder;

namespace LVK.Core.App.Web;

internal class WebApplicationBootstrapper : IApplicationBootstrapper<WebApplicationBuilder, WebApplication>
{
    public void Bootstrap(IHostBootstrapper<WebApplicationBuilder, WebApplication> bootstrapper, WebApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new ModuleBootstrapper());
    }
}