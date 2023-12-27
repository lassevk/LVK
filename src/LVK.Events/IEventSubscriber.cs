namespace LVK.Events;

public interface IEventSubscriber<in T>
{
    Task OnEvent(T evt, CancellationToken cancellationToken);
}