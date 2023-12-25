namespace LVK.Extensions.Bootstrapping;

/// <summary>
/// This interface can be used to bootstrap dependencies.
/// </summary>
/// <remarks>
/// This is the non-generic version of <see cref="IHostBootstrapper{TBuilder,THost}"/> and is typically used in class libraries
/// that are not written to be for any specific type of application.
/// </remarks>
public interface IHostBootstrapper
{
    /// <summary>
    /// Bootstrap a dependency.
    /// </summary>
    /// <param name="bootstrapper">
    /// The module bootstrapper responsible for bootstrapping said dependency, typically a class in the root namespace of a project, such as
    /// a class library.
    /// </param>
    /// <returns>
    /// The bootstrapper, used for chaining calls.
    /// </returns>
    IHostBootstrapper Bootstrap(IModuleBootstrapper bootstrapper);
}

/// <summary>
/// This interface can be used to bootstrap dependencies. An instance of this interface will be provided for
/// <see cref="IModuleBootstrapper{TBuilder,THost}"/> instances.
/// </summary>
/// <remarks>
/// This is the generic version of <see cref="IHostBootstrapper"/> and is typically used in the application project itself, and optionally
/// in any class libraries that are written for specific application types, such as a class library with blazor components.
/// </remarks>
/// <typeparam name="TBuilder">
/// The type of the builder that is in play.
/// </typeparam>
/// <typeparam name="THost">
/// The type of host that will be built.
/// </typeparam>
public interface IHostBootstrapper<TBuilder, THost> : IHostBootstrapper
{
    /// <summary>
    /// Bootstrap a dependency.
    /// </summary>
    /// <param name="bootstrapper">
    /// The module bootstrapper responsible for bootstrapping said dependency, typically a class in the root namespace of a project, such as
    /// a class library.
    /// </param>
    /// <returns>
    /// The bootstrapper, used for chaining calls.
    /// </returns>
    IHostBootstrapper<TBuilder, THost> Bootstrap(IModuleBootstrapper<TBuilder, THost> bootstrapper);
}