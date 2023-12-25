using LVK.Extensions.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NodaTime;

namespace LVK.NodaTime.Bootstrapped;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IClock>(SystemClock.Instance);
 
        builder.Services.AddKeyedSingleton(nameof(DateTimeZoneProviders.Bcl), DateTimeZoneProviders.Bcl);
        builder.Services.AddKeyedSingleton(nameof(DateTimeZoneProviders.Tzdb), DateTimeZoneProviders.Tzdb);
        builder.Services.AddSingleton(DateTimeZoneProviders.Bcl);
    }
}