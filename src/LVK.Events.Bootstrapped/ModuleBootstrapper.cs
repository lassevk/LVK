using LVK.Extensions.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Events.Bootstrapped;

/// <summary>
/// This is the module bootstrapper for LVK.Events.
/// </summary>
public class ModuleBootstrapper : IModuleBootstrapper
{
    /// <inheritdoc />
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IEventBus>(EventBus.Instance);
    }
}