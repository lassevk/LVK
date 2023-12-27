using Microsoft.Extensions.Hosting;

namespace LVK.Core.Bootstrapping;

public interface IModuleBootstrapper
{
    void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder);
}

public interface IApplicationBootstrapper<TBuilder, THost>
{
    void Bootstrap(IHostBootstrapper<TBuilder, THost> bootstrapper, TBuilder builder);
}
