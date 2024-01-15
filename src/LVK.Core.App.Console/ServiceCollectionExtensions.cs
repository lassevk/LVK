using System.Reflection;

using LVK.Core.App.Console.CommandLineInterface;
using LVK.Core.App.Console.CommandLineInterface.RoutableCommandsProviders;

using Microsoft.Extensions.DependencyInjection;

namespace LVK.Core.App.Console;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMainEntrypoint<T>(this IServiceCollection services)
        where T : class, IMainEntrypoint
        => services.AddTransient<IMainEntrypoint, T>();

    public static IServiceCollection RegisterRoutableCommandsInAssembly<T>(this IServiceCollection services)
        => services.AddSingleton(AssemblyRoutableCommandsProvider.Register(services, typeof(T).Assembly));

    public static IServiceCollection RegisterRoutableCommandsInAssembly(this IServiceCollection services, Assembly assembly)
        => services.AddSingleton(AssemblyRoutableCommandsProvider.Register(services, assembly));

    public static IServiceCollection RegisterRoutableCommand<T>(this IServiceCollection services)
        where T : ICommand
        => services.AddSingleton(TypeRoutableCommandProvider.Register(services, typeof(T)));

    public static IServiceCollection RegisterRoutableCommand(this IServiceCollection services, Type commandType) => services.AddSingleton(TypeRoutableCommandProvider.Register(services, commandType));
}