using Microsoft.Extensions.DependencyInjection;

namespace LVK.Extensions.Bootstrapping.Console;

/// <summary>
/// This class adds extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a type that implements <see cref="IMainEntrypoint"/> as
    /// the main application code.
    /// </summary>
    /// <remarks>
    /// After the <see cref="IMainEntrypoint.RunAsync"/> method has been
    /// executed on the type, the application will terminate.
    /// </remarks>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> in which to register the service.
    /// </param>
    /// <typeparam name="T">
    /// The type of the class implementing <see cref="IMainEntrypoint"/>.
    /// </typeparam>
    /// <returns>
    /// The <paramref name="services"/> instance, for method chaining.
    /// </returns>
    public static IServiceCollection AddMainEntrypoint<T>(this IServiceCollection services)
        where T : class, IMainEntrypoint
        => services.AddTransient<IMainEntrypoint, T>();
}