using LVK.Core.Collections;

namespace LVK.Data.Processing;

internal class EntityProcessorGraph<TInput, TOutput>
    where TInput : class
    where TOutput : class
{
    private readonly List<(IEntityProcessor processor, IEntityProcessor[] processorDependencies)> _processors;

    private EntityProcessorGraph(List<(IEntityProcessor processor, IEntityProcessor[] processorDependencies)> processors)
    {
        _processors = processors ?? throw new ArgumentNullException(nameof(processors));
    }

    public static EntityProcessorGraph<TInput, TOutput> Create(IEnumerable<IEntityProcessor> allProcessors)
    {
        var producedBy = new Dictionary<Type, IEntityProcessor>();
        var processors = allProcessors.ToList();

        foreach (IEntityProcessor processor in processors)
        {
            foreach (Type outputComponentType in processor.OutputTypes)
            {
                producedBy.Add(outputComponentType, processor);
            }
        }

        List<(IEntityProcessor item, IEntityProcessor dependency)> dependencies = OrderProcessorsByComponentDependencies(processors, producedBy, out List<IEntityProcessor> orderedProcessors);
        KeepOnlyRequiredProcessors(orderedProcessors);
        KeepOnlyProcessorsThatGetTheirInput(orderedProcessors);

        return new EntityProcessorGraph<TInput, TOutput>(orderedProcessors.Select(processor => (processor, dependencies.Where(dep => dep.item == processor).Select(dep => dep.dependency).ToArray())).ToList());
    }

    private static List<(IEntityProcessor item, IEntityProcessor dependency)> OrderProcessorsByComponentDependencies(List<IEntityProcessor> items, Dictionary<Type, IEntityProcessor> producedBy, out List<IEntityProcessor> ordered)
    {
        var dependencies = new List<(IEntityProcessor item, IEntityProcessor dependency)>();
        foreach (IEntityProcessor processor in items)
        {
            foreach (Type inputComponentType in processor.InputTypes)
            {
                if (!producedBy.TryGetValue(inputComponentType, out IEntityProcessor? producingProcessor))
                    continue;

                dependencies.Add((processor, producingProcessor));
            }
        }

        ordered = TopologicalSort.Sort(items, dependencies, new ObjectReferenceEqualityComparer<IEntityProcessor>()).ToList();
        return dependencies;
    }

    private static void KeepOnlyProcessorsThatGetTheirInput(List<IEntityProcessor> processors)
    {
        var possibleComponents = new HashSet<Type>
        {
            typeof(TInput),
        };

        var index = 0;
        while (index < processors.Count)
        {
            // Only include processor if at least one input type is possible to obtain at this point
            if (!processors[index].InputTypes.Any(inputType => possibleComponents.Contains(inputType)))
            {
                processors.RemoveAt(index);
                continue;
            }

            foreach (Type outputType in processors[index].OutputTypes)
                possibleComponents.Add(outputType);

            index++;
        }

        if (!possibleComponents.Contains(typeof(TOutput)))
            throw new InvalidOperationException($"Impossible to produce {typeof(TOutput)} from {typeof(TInput)} with the registered processors");
    }

    private static void KeepOnlyRequiredProcessors(List<IEntityProcessor> processors)
    {
        var possibleRequiredComponents = new HashSet<Type>
        {
            typeof(TOutput),
        };

        for (int index = processors.Count - 1; index >= 0; index--)
        {
            if (!processors[index].OutputTypes.Any(outputType => possibleRequiredComponents.Contains(outputType)))
            {
                // Processor is not required, so remove it from consideration
                processors.RemoveAt(index);
                continue;
            }

            foreach (Type inputType in processors[index].InputTypes)
                possibleRequiredComponents.Add(inputType);
        }

        if (!possibleRequiredComponents.Contains(typeof(TInput)))
            throw new InvalidOperationException($"{typeof(TInput)} is not required to produce {typeof(TOutput)} with the registered processors");
    }

    public EntityProcessorContext[] CreateTasks()
    {
        var result = _processors.ToDictionary(item => item.processor, item => new EntityProcessorContext(item.processor));

        foreach ((IEntityProcessor processor, IEntityProcessor[] processorDependencies) item in _processors)
        {
            EntityProcessorContext context = result[item.processor];
            foreach (IEntityProcessor dependency in item.processorDependencies)
            {
                EntityProcessorContext dependencyContext = result[dependency];
                context.Dependencies.Add(dependencyContext.ProcessorCompletionTaskSource.Task);
            }
        }

        return result.Values.ToArray();
    }
}