using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Extensions.Bootstrapping;

/// <summary>
/// This class adds extension methods to types implementing <see cref="IHostApplicationBuilder"/>.
/// </summary>
public static class HostApplicationBuilderExtensions
{
    /// <summary>
    /// Bootstrap the application builder using a module bootstrapper, and build and return the final host instance.
    /// </summary>
    /// <param name="builder">
    /// The type of builder to use to build the application host.
    /// </param>
    /// <param name="bootstrapper">
    /// An instance of <see cref="IModuleBootstrapper{TBuilder, THost}"/> that will be invoked to bootstrap the application and
    /// all dependencies.
    /// </param>
    /// <param name="build">
    /// A delegate used to build the application host. This will typically just be <c>b => b.Build()</c>.
    /// </param>
    /// <typeparam name="TBuilder">
    /// The type of builder to use to build the application host. This has to be a type that implements <see cref="IHostApplicationBuilder"/>, but
    /// is usually inferred from usage.
    /// </typeparam>
    /// <typeparam name="THost">
    /// The type of host that will be built. This has to be a type that implements <see cref="IHost"/>, but is usually inferred from usage.
    /// </typeparam>
    /// <returns>
    /// The built host object.
    /// </returns>
    public static THost BootstrapAndBuild<TBuilder, THost>(this TBuilder builder, IModuleBootstrapper<TBuilder, THost> bootstrapper, Func<TBuilder, THost> build)
        where TBuilder : IHostApplicationBuilder
        where THost : IHost
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(bootstrapper);
        ArgumentNullException.ThrowIfNull(build);

        IHostBootstrapper<TBuilder, THost> hostBootstrapper = new HostBootstrapper<TBuilder, THost>(builder);

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