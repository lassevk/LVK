namespace LVK.Core.Extensions;

public static class CollectionExtensions
{
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        Guard.NotNull(collection);
        Guard.NotNull(items);

        foreach (T item in items)
            collection.Add(item);
    }
}