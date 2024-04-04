namespace LVK.Data.Processing;

public interface IEntityProcessor
{
    IEnumerable<Type> InputTypes { get; }
    IEnumerable<Type> OutputTypes { get; }

    Task ProcessAsync(List<ProcessingEntity> entities, CancellationToken cancellationToken = default);
}