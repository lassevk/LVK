namespace LVK.Extensions.Bootstrapping;

/// <summary>
/// This interface can be optionally implemented on the same type that implement either
/// <see cref="IModuleBootstrapper{TBuilder,THost}"/> or <see cref="IModuleBootstrapper"/>, and if so, will
/// be invoked after the host instance has been built, but before it is returned from
/// <see cref="HostApplicationBuilderExtensions.BootstrapAndBuild{TBuilder,THost}"/>.
/// </summary>
/// <typeparam name="THost">
/// The type of host that has been built. Note that this type must correspond to either the
/// THost type on the <see cref="IModuleBootstrapper{TBuilder,THost}"/> interface, or IHost if implemented together
/// with <see cref="IModuleBootstrapper"/>.
/// </typeparam>
public interface IModuleInitializer<in THost>
{
    /// <summary>
    /// This method will be invoked to initialize the host, or any services, before the application starts. It will be invoked
    /// when the host has been built.
    /// </summary>
    /// <param name="host">
    /// The host instance that has been built.
    /// </param>
    void Initialize(THost host);
}