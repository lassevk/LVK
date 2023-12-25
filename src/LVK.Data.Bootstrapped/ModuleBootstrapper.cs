using LVK.Data.Configuration;
using LVK.Extensions.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Data.Bootstrapped;

/// <summary>
/// This is the module bootstrapper for LVK.Data and LVK.Data.Configuration.
/// </summary>
public class ModuleBootstrapper : IModuleBootstrapper
{
    /// <inheritdoc />
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDataProtectionPasswordProvider, EnvironmentVariableDataProtectionPasswordProvider>();
        builder.Services.AddSingleton<IDataProtectionPasswordProvider, ConfigurationDataProtectionPasswordProvider>();
        builder.Services.AddSingleton<IDataProtection, DataProtection>();
    }
}