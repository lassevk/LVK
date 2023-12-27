using System.ComponentModel;

using Microsoft.Extensions.Hosting;

namespace LVK.Core.Bootstrapping;

public interface IApplication
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    THost BootstrapAndBuild<TBuilder, THost>(TBuilder builder, Func<TBuilder, THost> build, params IApplicationBootstrapper<TBuilder, THost>[] bootstrappers)
        where TBuilder : IHostApplicationBuilder
        where THost : IHost;
}