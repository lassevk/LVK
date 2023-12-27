namespace LVK.Events;

internal interface IEventSubscriber<in T>
{
    Task OnEvent(T evt, CancellationToken cancellationToken);
}