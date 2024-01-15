using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace LVK.Core.App.Console.CommandLineInterface.RoutableCommandsProviders;

internal class AssemblyRoutableCommandsProvider : IRoutableCommandsProvider
{
    private readonly List<IRoutableCommand> _commands;

    private AssemblyRoutableCommandsProvider(List<IRoutableCommand> commands)
    {
        _commands = commands ?? throw new ArgumentNullException(nameof(commands));
    }

    public static IRoutableCommandsProvider Register(IServiceCollection services, Assembly assembly)
    {
        Guard.NotNull(services);
        Guard.NotNull(assembly);

        var commands = new List<IRoutableCommand>();

        Type baseType = typeof(ICommand);
        foreach (Type type in assembly.GetTypes())
        {
            if (type.IsAbstract)
                continue;

            if (!baseType.IsAssignableFrom(type))
                continue;

            services.AddTransient(type);
            commands.Add(new TypeRoutableCommand(type));
        }

        return new AssemblyRoutableCommandsProvider(commands);
    }

    public Task<List<IRoutableCommand>> GetCommandsAsync(CancellationToken cancellationToken) => Task.FromResult(_commands);
}