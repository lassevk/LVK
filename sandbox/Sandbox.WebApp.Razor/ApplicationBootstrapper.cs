using LVK.Core.Bootstrapping;

namespace Sandbox.WebApp.Razor;

public class ApplicationBootstrapper : IApplicationBootstrapper<WebApplicationBuilder, WebApplication>, IModuleInitializer<WebApplication>
{
    public void Bootstrap(IHostBootstrapper<WebApplicationBuilder, WebApplication> bootstrapper, WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();
    }

    public void Initialize(WebApplication host)
    {
        // Configure the HTTP request pipeline.
        if (!host.Environment.IsDevelopment())
        {
            host.UseExceptionHandler("/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            host.UseHsts();
        }

        host.UseHttpsRedirection();
        host.UseStaticFiles();

        host.UseRouting();

        host.UseAuthorization();

        host.MapRazorPages();

        host.Run();
    }
}