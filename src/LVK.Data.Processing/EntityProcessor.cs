namespace LVK.Data.Processing;

public static class EntityProcessor
{
    public static IEntityProcessor Create<TInput, TOutput>(Func<TInput, TOutput?> processor)
        where TInput : class
        where TOutput : class
        => Create<TInput, TOutput>((input, _) => Task.FromResult(processor(input)));

    public static IEntityProcessor Create<TInput, TOutput>(Func<TInput, Task<TOutput?>> processor)
        where TInput : class
        where TOutput : class
        => Create<TInput, TOutput>((input, _) => processor(input));

    public static IEntityProcessor Create<TInput, TOutput>(Func<TInput, CancellationToken, Task<TOutput?>> processor)
        where TInput : class
        where TOutput : class
        => new DelegateEntityProcessor<TInput, TOutput>(processor);
}