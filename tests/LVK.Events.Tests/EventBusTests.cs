using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace LVK.Events.Tests;

public class EventBusTests
{
#pragma warning disable NUnit1032
    private readonly IServiceProvider _serviceProvider = new ServiceCollection().BuildServiceProvider();
#pragma warning restore NUnit1032

    [Test]
    public async Task PublishEvent_SingleSubscriber_SubscriberIsInvoked()
    {
        var eventBus = new EventBus(_serviceProvider);
        string? received = null;
        eventBus.Subscribe<string>(s => received = s);
        await eventBus.PublishAsync("TEST");

        Assert.That(received, Is.EqualTo("TEST"));
    }

    [Test]
    public async Task PublishEvent_SingleSubscriberThatIsDisposed_SubscriberIsNoLongerInvoked()
    {
        var eventBus = new EventBus(_serviceProvider);
        string? received = null;
        IDisposable? subscription = eventBus.Subscribe<string>(s => received = s);
        subscription?.Dispose();
        await eventBus.PublishAsync("TEST");

        Assert.That(received, Is.Null);
    }

    [Test]
    public async Task PublishEvent_SubscriberAndMessageSameGroup_SubscriberIsInvoked()
    {
        var eventBus = new EventBus(_serviceProvider);
        string? received = null;
        object group = new();
        eventBus.Subscribe<string>(group, s => received = s);

        await eventBus.PublishAsync<string>(group, "TEST");
        Assert.That(received, Is.EqualTo("TEST"));
    }

    [Test]
    public async Task PublishEvent_SubscriberAndMessageSameGroupThatIsValueType_SubscriberIsInvoked()
    {
        var eventBus = new EventBus(_serviceProvider);
        string? received = null;
        eventBus.Subscribe<string>(10, s => received = s);

        await eventBus.PublishAsync<string>(10, "TEST");
        Assert.That(received, Is.EqualTo("TEST"));
   }

    [Test]
    public async Task PublishEvent_SubscriberInOneGroupMessageGoesToAnother_SubscriberIsNotInvoked()
    {
        var eventBus = new EventBus(_serviceProvider);
        string? received = null;
        eventBus.Subscribe<string>(new object(), s => received = s);

        await eventBus.PublishAsync<string>(new object(), "TEST");
        Assert.That(received, Is.Null);
    }

    [Test]
    public async Task PublishEvent_ServiceSubcriber_SubscriberIsInvoked()
    {
        var sc = new ServiceCollection();
        IEventSubscriber<string> subscriber = Substitute.For<IEventSubscriber<string>>()!;
        sc.AddSingleton(subscriber);
        var eventBus = new EventBus(sc.BuildServiceProvider());

        await eventBus.PublishAsync<string>("TEST");

        _ = subscriber.Received(1).OnEvent("TEST", Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task PublishEvent_KeyedServiceSubcriber_SubscriberIsInvoked()
    {
        var sc = new ServiceCollection();
        IEventSubscriber<string> subscriber = Substitute.For<IEventSubscriber<string>>()!;
        sc.AddKeyedSingleton("KEY", subscriber);
        var eventBus = new EventBus(sc.BuildServiceProvider());

        await eventBus.PublishAsync<string>("KEY", "TEST");

        _ = subscriber.Received(1).OnEvent("TEST", Arg.Any<CancellationToken>());
    }
}