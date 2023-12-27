namespace LVK.Core.Bootstrapping;

public interface IHostBootstrapper
{
    IHostBootstrapper Bootstrap(IModuleBootstrapper bootstrapper);
}

public interface IHostBootstrapper<TBuilder, THost> : IHostBootstrapper
{
    IHostBootstrapper<TBuilder, THost> Bootstrap(IApplicationBootstrapper<TBuilder, THost> bootstrapper);
}