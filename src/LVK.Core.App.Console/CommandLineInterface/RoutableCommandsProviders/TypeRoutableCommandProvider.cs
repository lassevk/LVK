using Microsoft.Extensions.DependencyInjection;

namespace LVK.Core.App.Console.CommandLineInterface.RoutableCommandsProviders;

internal class TypeRoutableCommandProvider : IRoutableCommandsProvider
{
    private readonly TypeRoutableCommand _command;

    private TypeRoutableCommandProvider(TypeRoutableCommand command)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
    }

    public Task<List<IRoutableCommand>> GetCommandsAsync(CancellationToken cancellationToken) => Task.FromResult<List<IRoutableCommand>>([_command]);

    public static IRoutableCommandsProvider Register(IServiceCollection services, Type commandType)
    {
        Guard.NotNull(services);
        Guard.NotNull(commandType);
        Guard.Against(commandType.IsAbstract);
        Guard.Assert(typeof(ICommand).IsAssignableFrom(commandType));

        services.AddTransient(commandType);
        return new TypeRoutableCommandProvider(new TypeRoutableCommand(commandType));
    }
}