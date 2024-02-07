using Microsoft.Extensions.Hosting;

namespace LVK.Core.Bootstrapping;

public interface IModuleBootstrapper
{
    void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder);
}