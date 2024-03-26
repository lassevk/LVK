using LVK.Core.Bootstrapping;
using LVK.Data.Protection;
using LVK.Data.Protection.PasswordProviders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Data;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        bootstrapper.Bootstrap(new LVK.Core.ModuleBootstrapper());

        builder.Services.AddSingleton<IDataProtectionPasswordProvider, EnvironmentVariableDataProtectionPasswordProvider>();
        builder.Services.AddSingleton<IDataProtectionPasswordProvider, ConfigurationDataProtectionPasswordProvider>();
        builder.Services.AddSingleton<IDataProtection, DataProtection>();
    }
}