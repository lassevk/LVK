namespace LVK;

/// <summary>
/// This class adds some extension methods for <see cref="ICollection{T}"/>.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Adds a given set of items to the collection.
    /// </summary>
    /// <param name="collection">
    /// The <see cref="ICollection{T}"/> to add items to.
    /// </param>
    /// <param name="items">
    /// The items to add to the <paramref name="collection"/>.
    /// </param>
    /// <typeparam name="T">
    /// The type of items.
    /// </typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> is <c>null</c>.
    /// <para>- or -</para>
    /// <paramref name="items"/> is <c>null</c>.
    /// </exception>
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        Guard.NotNull(collection);
        Guard.NotNull(items);

        foreach (T item in items)
            collection.Add(item);
    }
}