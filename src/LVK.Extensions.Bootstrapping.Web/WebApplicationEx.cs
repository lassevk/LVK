using Microsoft.AspNetCore.Builder;

namespace LVK.Extensions.Bootstrapping.Web;

/// <summary>
/// This class implements a different way of configuring web applications than through the
/// <see cref="WebApplication"/> builder factory.+
/// </summary>
public static class WebApplicationEx
{
    /// <inheritdoc cref="WebApplication.CreateBuilder()"/>
    public static WebApplication Create<TModuleBootstrapper>()
        where TModuleBootstrapper : IModuleBootstrapper<WebApplicationBuilder, WebApplication>, new()
    {
        var moduleBootstrapper = new TModuleBootstrapper();
        return WebApplication.CreateBuilder().BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateBuilder(WebApplicationOptions)"/>
    public static WebApplication Create<TModuleBootstrapper>(WebApplicationOptions options)
        where TModuleBootstrapper : IModuleBootstrapper<WebApplicationBuilder, WebApplication>, new()
    {
        var moduleBootstrapper = new TModuleBootstrapper();
        return WebApplication.CreateBuilder(options).BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateBuilder(string[])"/>
    public static WebApplication Create<TModuleBootstrapper>(string[] args)
        where TModuleBootstrapper : IModuleBootstrapper<WebApplicationBuilder, WebApplication>, new()
    {
        var moduleBootstrapper = new TModuleBootstrapper();
        return WebApplication.CreateBuilder(args).BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateEmptyBuilder(WebApplicationOptions)"/>
    public static WebApplication CreateEmpty<TModuleBootstrapper>(WebApplicationOptions options)
        where TModuleBootstrapper : IModuleBootstrapper<WebApplicationBuilder, WebApplication>, new()
    {
        var moduleBootstrapper = new TModuleBootstrapper();
        return WebApplication.CreateEmptyBuilder(options).BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateSlimBuilder()"/>
    public static WebApplication CreateSlim<TModuleBootstrapper>()
        where TModuleBootstrapper : IModuleBootstrapper<WebApplicationBuilder, WebApplication>, new()
    {
        var moduleBootstrapper = new TModuleBootstrapper();
        return WebApplication.CreateSlimBuilder().BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateSlimBuilder(WebApplicationOptions)"/>
    public static WebApplication CreateSlim<TModuleBootstrapper>(WebApplicationOptions options)
        where TModuleBootstrapper : IModuleBootstrapper<WebApplicationBuilder, WebApplication>, new()
    {
        var moduleBootstrapper = new TModuleBootstrapper();
        return WebApplication.CreateSlimBuilder(options).BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateSlimBuilder(string[])"/>
    public static WebApplication CreateSlim<TModuleBootstrapper>(string[] args)
        where TModuleBootstrapper : IModuleBootstrapper<WebApplicationBuilder, WebApplication>, new()
    {
        var moduleBootstrapper = new TModuleBootstrapper();
        return WebApplication.CreateSlimBuilder(args).BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateBuilder()"/>
    public static WebApplication Create(IModuleBootstrapper<WebApplicationBuilder, WebApplication> moduleBootstrapper)
    {
        return WebApplication.CreateBuilder().BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateBuilder(WebApplicationOptions)"/>
    public static WebApplication Create(WebApplicationOptions options, IModuleBootstrapper<WebApplicationBuilder, WebApplication> moduleBootstrapper)
    {
        return WebApplication.CreateBuilder(options).BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateBuilder(string[])"/>
    public static WebApplication Create(string[] args, IModuleBootstrapper<WebApplicationBuilder, WebApplication> moduleBootstrapper)
    {
        return WebApplication.CreateBuilder(args).BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateEmptyBuilder(WebApplicationOptions)"/>
    public static WebApplication CreateEmpty(WebApplicationOptions options, IModuleBootstrapper<WebApplicationBuilder, WebApplication> moduleBootstrapper)
    {
        return WebApplication.CreateEmptyBuilder(options).BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateSlimBuilder()"/>
    public static WebApplication CreateSlim(IModuleBootstrapper<WebApplicationBuilder, WebApplication> moduleBootstrapper)
    {
        return WebApplication.CreateSlimBuilder().BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateSlimBuilder(WebApplicationOptions)"/>
    public static WebApplication CreateSlim(WebApplicationOptions options, IModuleBootstrapper<WebApplicationBuilder, WebApplication> moduleBootstrapper)
    {
        return WebApplication.CreateSlimBuilder(options).BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }

    /// <inheritdoc cref="WebApplication.CreateSlimBuilder(string[])"/>
    public static WebApplication CreateSlim(string[] args, IModuleBootstrapper<WebApplicationBuilder, WebApplication> moduleBootstrapper)
    {
        return WebApplication.CreateSlimBuilder(args).BootstrapAndBuild(moduleBootstrapper, b => b.Build());
    }
}