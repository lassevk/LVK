using Nito.AsyncEx;

namespace LVK.Data.Processing;

internal class Processor<TInput, TOutput> : IProcessor<TInput, TOutput>
    where TInput : class
    where TOutput : class
{
    private readonly IEnumerable<IEntityProcessorProvider> _processorProviders;
    private readonly List<IEntityProcessor> _processors;
    private EntityProcessorGraph<TInput, TOutput>? _graph;
    private readonly AsyncLock _lock = new();

    public Processor(IEnumerable<IEntityProcessor> processors, IEnumerable<IEntityProcessorProvider> processorProviders)
    {
        if (processors == null)
            throw new ArgumentNullException(nameof(processors));

        _processorProviders = processorProviders ?? throw new ArgumentNullException(nameof(processorProviders));
        _processors = processors.ToList();
    }

    public async Task<List<(TInput input, TOutput? output)>> ProcessAsync(IEnumerable<TInput> inputs, CancellationToken cancellationToken = default)
    {
        EntityProcessorContext[] processorContexts = await CreateProcessorContexts(cancellationToken);
        List<ProcessingEntity> entities = await CreateProcessingEntities(inputs, cancellationToken);
        await ProcessAllEntities(cancellationToken, processorContexts, entities);
        return await ExtractOutputComponents(cancellationToken, entities);
    }

    private static async Task<List<(TInput input, TOutput? output)>> ExtractOutputComponents(CancellationToken cancellationToken, List<ProcessingEntity> entities)
    {
        var result = new List<(TInput input, TOutput? output)>();
        foreach (ProcessingEntity entity in entities)
        {
            TInput input = (await entity.TryGetComponentAsync<TInput>(cancellationToken))!;
            TOutput? output = await entity.TryGetComponentAsync<TOutput>(cancellationToken);

            result.Add((input, output));
        }

        return result;
    }

    private async Task ProcessAllEntities(CancellationToken cancellationToken, EntityProcessorContext[] processorContexts, List<ProcessingEntity> entities) => await Task.WhenAll(processorContexts.Select(context => RunProcessorOnEntities(context, entities, cancellationToken)));

    private static async Task<List<ProcessingEntity>> CreateProcessingEntities(IEnumerable<TInput> inputs, CancellationToken cancellationToken)
    {
        var entities = new List<ProcessingEntity>();
        foreach (TInput input in inputs)
        {
            var entity = new ProcessingEntity();
            await entity.SetComponentAsync(input, cancellationToken);
            entities.Add(entity);
        }

        return entities;
    }

    private async Task<EntityProcessorContext[]> CreateProcessorContexts(CancellationToken cancellationToken)
    {
        if (_graph == null)
        {
            using (await _lock.LockAsync(cancellationToken))
            {
                if (_graph == null)
                {
                    var processors = _processors.ToList();
                    foreach (IEntityProcessorProvider provider in _processorProviders)
                    {
                        await foreach (IEntityProcessor processor in provider.ProvideAsync(cancellationToken))
                            processors.Add(processor);
                    }

                    _graph = EntityProcessorGraph<TInput, TOutput>.Create(processors);
                }
            }
        }

        return _graph.CreateTasks();
    }

    private async Task RunProcessorOnEntities(EntityProcessorContext context, List<ProcessingEntity> entities, CancellationToken cancellationToken)
    {
        try
        {
            await Task.WhenAll(context.Dependencies);
            await context.Processor.ProcessAsync(entities, cancellationToken);
            context.ProcessorCompletionTaskSource.SetResult();
        }
        catch (TaskCanceledException)
        {
            context.ProcessorCompletionTaskSource.SetCanceled(cancellationToken);
            throw;
        }
        catch (Exception ex)
        {
            context.ProcessorCompletionTaskSource.SetException(ex);
            throw;
        }
    }
}