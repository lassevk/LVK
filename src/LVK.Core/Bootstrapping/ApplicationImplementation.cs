using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Core.Bootstrapping;

internal class ApplicationImplementation : IApplication
{
    public THost BootstrapAndBuild<TBuilder, THost>(TBuilder builder, Func<TBuilder, THost> build, params IApplicationBootstrapper<TBuilder, THost>[] bootstrappers)
        where TBuilder : IHostApplicationBuilder
        where THost : IHost
    {
        Guard.NotNull(builder);
        Guard.NotNull(build);
        Guard.NotNull(bootstrappers);

        IHostBootstrapper<TBuilder, THost> hostBootstrapper = new HostBootstrapper<TBuilder, THost>(builder);

        foreach (IApplicationBootstrapper<TBuilder, THost> bootstrapper in bootstrappers)
            hostBootstrapper.Bootstrap(bootstrapper);

        THost host = build(builder);
        Initialize(host);

        return host;
    }

    private static void Initialize<THost>(THost host)
        where THost : IHost
    {
        var initializers = new HashSet<object>();

        // Configure host
        foreach (IModuleInitializer<IHost> initializer in host.Services.GetServices<IModuleInitializer<IHost>>())
        {
            if (initializers.Add(initializer))
                initializer.Initialize(host);
        }

        foreach (IModuleInitializer<THost> initializer in host.Services.GetServices<IModuleInitializer<THost>>())
        {
            if (initializers.Add(initializer))
                initializer.Initialize(host);
        }
    }
}