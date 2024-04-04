namespace LVK.Data.Processing;

public interface IProcessor<TInput, TOutput>
    where TInput : class
    where TOutput : class
{
    Task<List<(TInput input, TOutput? output)>> ProcessAsync(IEnumerable<TInput> inputs, CancellationToken cancellationToken = default);
}