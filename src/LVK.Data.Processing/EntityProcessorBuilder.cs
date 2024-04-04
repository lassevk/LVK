namespace LVK.Data.Processing;

public class EntityProcessorBuilder
{
    private readonly HashSet<Type> _inputTypes = new();
    private readonly HashSet<Type> _outputTypes = new();
    private readonly List<Func<List<ProcessingEntity>, CancellationToken, Task>> _actions = new();

    public EntityProcessorBuilder Add<TInput, TOutput>(Func<TInput, CancellationToken, Task<TOutput?>> action)
        where TInput : class
        where TOutput : class
    {
        _inputTypes.Add(typeof(TInput));
        _outputTypes.Add(typeof(TOutput));
        _actions.Add(async (entities, cancellationToken) =>
        {
            await Task.WhenAll(entities.Select(async entity =>
            {
                TInput? input = await entity.TryGetComponentAsync<TInput>(cancellationToken);
                if (input is null)
                    return;

                TOutput? output = await action(input, cancellationToken);
                if (output is null)
                    return;

                await entity.AddComponentAsync(output, cancellationToken);
            }));
        });

        return this;
    }

    public EntityProcessorBuilder Add<TInput, TOutput>(Func<TInput, Task<TOutput?>> action)
        where TInput : class
        where TOutput : class
        => Add(async (TInput input, CancellationToken _) => await action(input));

    public EntityProcessorBuilder Add<TInput, TOutput>(Func<TInput, TOutput?> action)
        where TInput : class
        where TOutput : class
        => Add((TInput input, CancellationToken _) => Task.FromResult(action(input)));

    public IEntityProcessor Build()
    {
        if (_actions.Count == 0)
            throw new InvalidOperationException("No actions registered for this entity processor");

        return new CompoundEntityProcessor(_inputTypes.ToArray(), _outputTypes.ToArray(), _actions.ToArray());
    }
}