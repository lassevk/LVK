using Microsoft.Extensions.Hosting;

namespace LVK.Extensions.Bootstrapping.Console;

/// <summary>
/// This class implements a different way of configuring console applications than through the
/// <see cref="Host"/> builder factory.+
/// </summary>
public static class HostEx
{
    /// <inheritdoc cref="Host.CreateApplicationBuilder()"/>
    public static IHost CreateApplication<TModuleBootstrapper>()
        where TModuleBootstrapper : IModuleBootstrapper<HostApplicationBuilder, IHost>, new()
    {
        var moduleBootstrapper = new TModuleBootstrapper();
        var wrapper = new ModuleBootstrapperWrapper(moduleBootstrapper);
        return Host.CreateApplicationBuilder().BootstrapAndBuild(wrapper, b => b.Build());
    }

    /// <inheritdoc cref="Host.CreateApplicationBuilder(string[])"/>
    public static IHost CreateApplication<TModuleBootstrapper>(string[] args)
        where TModuleBootstrapper : IModuleBootstrapper<HostApplicationBuilder, IHost>, new()
    {
        var moduleBootstrapper = new TModuleBootstrapper();
        var wrapper = new ModuleBootstrapperWrapper(moduleBootstrapper);
        return Host.CreateApplicationBuilder(args).BootstrapAndBuild(wrapper, b => b.Build());
    }

    /// <inheritdoc cref="Host.CreateApplicationBuilder(HostApplicationBuilderSettings)"/>
    public static IHost CreateApplication<TModuleBootstrapper>(HostApplicationBuilderSettings? settings)
        where TModuleBootstrapper : IModuleBootstrapper<HostApplicationBuilder, IHost>, new()
    {
        var moduleBootstrapper = new TModuleBootstrapper();
        var wrapper = new ModuleBootstrapperWrapper(moduleBootstrapper);
        return Host.CreateApplicationBuilder(settings).BootstrapAndBuild(wrapper, b => b.Build());
    }

    /// <inheritdoc cref="Host.CreateEmptyApplicationBuilder(HostApplicationBuilderSettings)"/>
    public static IHost CreateEmptyApplication<TModuleBootstrapper>(HostApplicationBuilderSettings? settings)
        where TModuleBootstrapper : IModuleBootstrapper<HostApplicationBuilder, IHost>, new()
    {
        var moduleBootstrapper = new TModuleBootstrapper();
        var wrapper = new ModuleBootstrapperWrapper(moduleBootstrapper);
        return Host.CreateEmptyApplicationBuilder(settings).BootstrapAndBuild(wrapper, b => b.Build());
    }

    /// <inheritdoc cref="Host.CreateApplicationBuilder()"/>
    public static IHost CreateApplication(IModuleBootstrapper<HostApplicationBuilder, IHost> moduleBootstrapper)
    {
        var wrapper = new ModuleBootstrapperWrapper(moduleBootstrapper);
        return Host.CreateApplicationBuilder().BootstrapAndBuild(wrapper, b => b.Build());
    }

    /// <inheritdoc cref="Host.CreateApplicationBuilder(string[])"/>
    public static IHost CreateApplication(string[] args, IModuleBootstrapper<HostApplicationBuilder, IHost> moduleBootstrapper)
    {
        var wrapper = new ModuleBootstrapperWrapper(moduleBootstrapper);
        return Host.CreateApplicationBuilder(args).BootstrapAndBuild(wrapper, b => b.Build());
    }

    /// <inheritdoc cref="Host.CreateApplicationBuilder(HostApplicationBuilderSettings)"/>
    public static IHost CreateApplication(HostApplicationBuilderSettings? settings, IModuleBootstrapper<HostApplicationBuilder, IHost> moduleBootstrapper)
    {
        var wrapper = new ModuleBootstrapperWrapper(moduleBootstrapper);
        return Host.CreateApplicationBuilder(settings).BootstrapAndBuild(wrapper, b => b.Build());
    }

    /// <inheritdoc cref="Host.CreateEmptyApplicationBuilder(HostApplicationBuilderSettings)"/>
    public static IHost CreateEmptyApplication(HostApplicationBuilderSettings? settings, IModuleBootstrapper<HostApplicationBuilder, IHost> moduleBootstrapper)
    {
        var wrapper = new ModuleBootstrapperWrapper(moduleBootstrapper);
        return Host.CreateEmptyApplicationBuilder(settings).BootstrapAndBuild(wrapper, b => b.Build());
    }
}