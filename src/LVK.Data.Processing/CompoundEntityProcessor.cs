namespace LVK.Data.Processing;

internal class CompoundEntityProcessor : IEntityProcessor
{
    private readonly Func<List<ProcessingEntity>, CancellationToken, Task>[] _actions;

    public CompoundEntityProcessor(Type[] inputTypes, Type[] outputTypes, Func<List<ProcessingEntity>,CancellationToken,Task>[] actions)
    {
        _actions = actions ?? throw new ArgumentNullException(nameof(actions));
        InputTypes = inputTypes ?? throw new ArgumentNullException(nameof(inputTypes));
        OutputTypes = outputTypes ?? throw new ArgumentNullException(nameof(outputTypes));
    }

    public IEnumerable<Type> InputTypes { get; }
    public IEnumerable<Type> OutputTypes { get; }
    public async Task ProcessAsync(List<ProcessingEntity> entities, CancellationToken cancellationToken = default)
    {
        await Task.WhenAll(_actions.Select(action => action(entities, cancellationToken)));
    }
}