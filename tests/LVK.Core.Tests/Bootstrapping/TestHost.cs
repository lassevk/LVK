using Microsoft.Extensions.Hosting;

namespace LVK.Core.Tests.Bootstrapping;

public class TestHost : IHost
{
    public TestHost(IServiceProvider serviceProvider)
    {
        Services = serviceProvider;
    }

    public void Dispose() { }

    public Task StartAsync(CancellationToken cancellationToken = new CancellationToken()) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken = new CancellationToken()) => Task.CompletedTask;

    public IServiceProvider Services { get; }
}