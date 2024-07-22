using LVK.Core.Bootstrapping;

using App = Sandbox.WebApp.Blazor.Components.App;

namespace Sandbox.WebApp.Blazor;

public class ApplicationBootstrapper : IApplicationBootstrapper<WebApplicationBuilder, WebApplication>, IModuleInitializer<WebApplication>
{
    public void Bootstrap(IHostBootstrapper<WebApplicationBuilder, WebApplication> bootstrapper, WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
    }

    public void Initialize(WebApplication host)
    {
        // Configure the HTTP request pipeline.
        if (!host.Environment.IsDevelopment())
        {
            host.UseExceptionHandler("/Error", true);

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            host.UseHsts();
        }

        host.UseHttpsRedirection();

        host.UseStaticFiles();
        host.UseAntiforgery();

        host.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        host.Run();
    }
}