using Microsoft.Extensions.Hosting;

namespace LVK.Extensions.Bootstrapping;

/// <summary>
/// This interface can be implemented by module bootstrapper for projects.
/// </summary>
/// <remarks>
/// This is the non-generic version of <see cref="IModuleBootstrapper{TBuilder,THost}"/> and is typically implemented for
/// module bootstrappers in class libraries not specifically written for any particular type of application.
/// </remarks>
public interface IModuleBootstrapper
{
    /// <summary>
    /// Bootstrap the module.
    /// </summary>
    /// <param name="bootstrapper">
    /// The <see cref="IHostBootstrapper"/> object, through which dependencies can be bootstrapped.
    /// </param>
    /// <param name="builder">
    /// The <see cref="IHostApplicationBuilder"/> builder object that must be configured.
    /// </param>
    void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder);
}

/// <summary>
/// This interface can be implemented by module bootstrapper for projects.
/// </summary>
/// <remarks>
/// This is the generic version of <see cref="IModuleBootstrapper"/> and is typically implemented for
/// module bootstrappers in the main application projects, or class libraries specifically written for a type of application, such
/// as a class library with blazor components.
/// </remarks>
/// <typeparam name="TBuilder">
/// The type of the builder that is in play.
/// </typeparam>
/// <typeparam name="THost">
/// The type of host that will be built.
/// </typeparam>
public interface IModuleBootstrapper<TBuilder, THost>
{
    /// <summary>
    /// Bootstrap the module.
    /// </summary>
    /// <param name="bootstrapper">
    /// The <see cref="IHostBootstrapper{TBuilder,THost}"/> object, through which dependencies can be bootstrapped.
    /// </param>
    /// <param name="builder">
    /// The <see cref="IHostApplicationBuilder"/> builder object that must be configured.
    /// </param>
    void Bootstrap(IHostBootstrapper<TBuilder, THost> bootstrapper, TBuilder builder);
}
