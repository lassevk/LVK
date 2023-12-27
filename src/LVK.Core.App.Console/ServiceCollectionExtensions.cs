using Microsoft.Extensions.DependencyInjection;

namespace LVK.Core.App.Console;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMainEntrypoint<T>(this IServiceCollection services)
        where T : class, IMainEntrypoint
        => services.AddTransient<IMainEntrypoint, T>();
}