using Microsoft.Extensions.DependencyInjection;

namespace LVK.Events.Tests;

public class WeakEventTest
{
#pragma warning disable NUnit1032
    private readonly IServiceProvider _serviceProvider = new ServiceCollection().BuildServiceProvider();
#pragma warning restore NUnit1032

    public static int Counter;

    [Test]
    public async Task PublishEvent_SubscriberStillAlive_HandlesEvent()
    {
        IEventBus bus = new EventBus(_serviceProvider);
        var subscriber = new Subscriber();
        IDisposable subscription = bus.Subscribe(null, subscriber);

        Counter = 0;
        await bus.PublishAsync("Test");
        Assert.That(Counter, Is.EqualTo(1));

        GC.KeepAlive(subscriber);
    }

    [Test]
    public async Task PublishEvent_SubscriberGarbageCollected_DoesNotHandleEvent()
    {
        IEventBus bus = new EventBus(_serviceProvider);
        await AttachSubscriber(bus);

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Counter = 0;
        await bus.PublishAsync("Test");
        Assert.That(Counter, Is.EqualTo(0));
    }

    private async Task AttachSubscriber(IEventBus bus)
    {
        var subscriber = new Subscriber();
        bus.Subscribe(null, subscriber);

        Counter = 0;
        await bus.PublishAsync("Test");
        Assert.That(Counter, Is.EqualTo(1));
    }

    public class Subscriber : IEventSubscriber<string>
    {
        public Task OnEvent(string evt, CancellationToken cancellationToken) => Task.FromResult(Counter++);
    }
}