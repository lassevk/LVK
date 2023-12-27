using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Core.Bootstrapping;

internal class HostBootstrapper<TBuilder, THost> : IHostBootstrapper<TBuilder, THost>
    where TBuilder : IHostApplicationBuilder
    where THost : IHost
{
    private readonly HashSet<Type> _bootstrappedTypes = new();
    private readonly TBuilder _builder;
    private readonly HashSet<Type> _inflightBootstrapperTypes = new();

    internal HostBootstrapper(TBuilder builder)
    {
        Guard.NotNull(builder);

        _builder = builder;
    }

    IHostBootstrapper IHostBootstrapper.Bootstrap(IModuleBootstrapper bootstrapper)
    {
        AddInflightBootstrapper(bootstrapper);
        try
        {
            Guard.NotNull(bootstrapper);

            if (!_bootstrappedTypes.Add(bootstrapper.GetType()))
                return this;

            bootstrapper.Bootstrap(this, _builder);

            if (bootstrapper is IModuleInitializer<IHost> initializer)
                _builder.Services.AddSingleton(initializer);

            return this;
        }
        finally
        {
            RemoveInflightBootstrapper(bootstrapper);
        }
    }

    private void AddInflightBootstrapper(object bootstrapper)
    {
        if (_inflightBootstrapperTypes.Add(bootstrapper.GetType()))
            return;

        throw new InvalidOperationException($"Bootstrapper type {bootstrapper.GetType().FullName} is recursively being bootstrapped twice");
    }

    private void RemoveInflightBootstrapper(object bootstrapper)
    {
        _inflightBootstrapperTypes.Remove(bootstrapper.GetType());
    }

    IHostBootstrapper<TBuilder, THost> IHostBootstrapper<TBuilder, THost>.Bootstrap(IApplicationBootstrapper<TBuilder, THost> bootstrapper)
    {
        AddInflightBootstrapper(bootstrapper);
        try
        {
            Guard.NotNull(bootstrapper);

            if (!_bootstrappedTypes.Add(bootstrapper.GetType()))
                return this;

            bootstrapper.Bootstrap(this, _builder);

            if (bootstrapper is IModuleInitializer<THost> initializer)
                _builder.Services.AddSingleton(initializer);

            return this;
        }
        finally
        {
            RemoveInflightBootstrapper(bootstrapper);
        }
    }
}