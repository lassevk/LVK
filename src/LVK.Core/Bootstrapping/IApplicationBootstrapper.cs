namespace LVK.Core.Bootstrapping;

public interface IApplicationBootstrapper<TBuilder, THost>
{
    void Bootstrap(IHostBootstrapper<TBuilder, THost> bootstrapper, TBuilder builder);
}
