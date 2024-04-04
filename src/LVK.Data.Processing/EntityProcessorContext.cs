namespace LVK.Data.Processing;

internal class EntityProcessorContext
{
    public EntityProcessorContext(IEntityProcessor processor)
    {
        Processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    public IEntityProcessor Processor { get; }
    public List<Task> Dependencies { get; } = new();
    public TaskCompletionSource ProcessorCompletionTaskSource { get; } = new();
}