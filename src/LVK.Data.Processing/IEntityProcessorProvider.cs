namespace LVK.Data.Processing;

public interface IEntityProcessorProvider
{
    IAsyncEnumerable<IEntityProcessor> ProvideAsync(CancellationToken cancellationToken);
}