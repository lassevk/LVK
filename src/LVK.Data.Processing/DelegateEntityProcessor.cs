namespace LVK.Data.Processing;

internal class DelegateEntityProcessor<TInput, TOutput> : IEntityProcessor
    where TInput : class
    where TOutput : class
{
    private readonly Func<TInput, CancellationToken, Task<TOutput?>> _processor;

    public DelegateEntityProcessor(Func<TInput, CancellationToken, Task<TOutput?>> processor)
    {
        _processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    public IEnumerable<Type> InputTypes => [typeof(TInput)];
    public IEnumerable<Type> OutputTypes => [typeof(TOutput)];
    public async Task ProcessAsync(List<ProcessingEntity> entities, CancellationToken cancellationToken = default)
    {
        await Task.WhenAll(entities.Select(async entity =>
            {
                TInput? input = await entity.TryGetComponentAsync<TInput>(cancellationToken);
                if (input is null)
                    return;

                TOutput? output = await _processor(input, cancellationToken);
                if (output is null)
                    return;

                await entity.AddComponentAsync(output, cancellationToken);
            }));
    }
}