namespace LVK;

/// <summary>
/// Provide extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Copy the <see cref="IEnumerable{T}"/> collection to a new array and sort it.
    /// </summary>
    /// <param name="collection">
    /// The collection to copy to a new array.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> comparer to use for the sorting. This parameter is optional,
    /// and default sorting rules will be applied if omitted.
    /// </param>
    /// <typeparam name="T">
    /// The type of elements in the collection and resulting array.
    /// </typeparam>
    /// <returns>
    /// The array that was constructed and sorted.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> is <c>null</c>.
    /// </exception>
    public static T[] ToOrderedArray<T>(this IEnumerable<T> collection, IComparer<T>? comparer = null)
        where T: IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(collection);

        T[] result = collection.ToArray();
        Array.Sort(result, comparer);
        return result;
    }

    /// <summary>
    /// Copy the <see cref="IEnumerable{T}"/> collection to a new <see cref="List{T}"/> and sort it.
    /// </summary>
    /// <param name="collection">
    /// The collection to copy to a new <see cref="List{T}"/>.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> comparer to use for the sorting. This parameter is optional,
    /// and default sorting rules will be applied if omitted.
    /// </param>
    /// <typeparam name="T">
    /// The type of elements in the collection and resulting <see cref="List{T}"/>.
    /// </typeparam>
    /// <returns>
    /// The <see cref="List{T}"/> that was constructed and sorted.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> is <c>null</c>.
    /// </exception>
    public static List<T> ToOrderedList<T>(this IEnumerable<T> collection, IComparer<T>? comparer = null)
        where T: IComparable<T>
    {
        var result = collection.ToList();
        result.Sort(comparer);
        return result;
    }

    /// <summary>
    /// Copy the <see cref="IEnumerable{T}"/> collection to a new <see cref="List{T}"/> and sort it.
    /// </summary>
    /// <param name="collection">
    /// The collection to copy to a new <see cref="List{T}"/>.
    /// </param>
    /// <param name="comparison">
    /// The <see cref="Comparison{T}"/> comparer to use for the sorting.
    /// </param>
    /// <typeparam name="T">
    /// The type of elements in the collection and resulting <see cref="List{T}"/>.
    /// </typeparam>
    /// <returns>
    /// The <see cref="List{T}"/> that was constructed and sorted.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> is <c>null</c>.
    /// </exception>
    public static List<T> ToOrderedList<T>(this IEnumerable<T> collection, Comparison<T> comparison)
        where T: IComparable<T>
    {
        var result = collection.ToList();
        result.Sort(comparison);
        return result;
    }
}