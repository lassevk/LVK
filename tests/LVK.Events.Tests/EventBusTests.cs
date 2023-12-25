namespace LVK.Events.Tests;

public class EventBusTests
{
    [Test]
    public async Task PublishEvent_SingleSubscriber_SubscriberIsInvoked()
    {
        var eventBus = new EventBus();
        string? received = null;
        eventBus.Subscribe<string>(s => received = s);
        await eventBus.PublishAsync("TEST");

        Assert.That(received, Is.EqualTo("TEST"));
    }

    [Test]
    public async Task PublishEvent_SingleSubscriberThatIsDisposed_SubscriberIsNoLongerInvoked()
    {
        var eventBus = new EventBus();
        string? received = null;
        IDisposable? subscription = eventBus.Subscribe<string>(s => received = s);
        subscription?.Dispose();
        await eventBus.PublishAsync("TEST");

        Assert.That(received, Is.Null);
    }

    [Test]
    public async Task PublishEvent_SubscriberAndMessageSameGroup_SubscriberIsInvoked()
    {
        var eventBus = new EventBus();
        string? received = null;
        object group = new();
        eventBus.Subscribe<string>(group, s => received = s);

        await eventBus.PublishAsync<string>(group, "TEST");
        Assert.That(received, Is.EqualTo("TEST"));

    }

    [Test]
    public async Task PublishEvent_SubscriberAndMessageSameGroupThatIsValueType_SubscriberIsInvoked()
    {
        var eventBus = new EventBus();
        string? received = null;
        eventBus.Subscribe<string>(10, s => received = s);

        await eventBus.PublishAsync<string>(10, "TEST");
        Assert.That(received, Is.EqualTo("TEST"));

    }

    [Test]
    public async Task PublishEvent_SubscriberInOneGroupMessageGoesToAnother_SubscriberIsNotInvoked()
    {
        var eventBus = new EventBus();
        string? received = null;
        eventBus.Subscribe<string>(new object(), s => received = s);

        await eventBus.PublishAsync<string>(new object(), "TEST");
        Assert.That(received, Is.Null);
    }
}