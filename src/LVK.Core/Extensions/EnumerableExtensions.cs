namespace LVK.Core.Extensions;

public static class EnumerableExtensions
{
    public static T[] ToOrderedArray<T>(this IEnumerable<T> collection, IComparer<T>? comparer = null)
        where T : IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(collection);

        T[] result = collection.ToArray();
        Array.Sort(result, comparer);
        return result;
    }

    public static List<T> ToOrderedList<T>(this IEnumerable<T> collection, IComparer<T>? comparer = null)
        where T : IComparable<T>
    {
        var result = collection.ToList();
        result.Sort(comparer);
        return result;
    }

    public static List<T> ToOrderedList<T>(this IEnumerable<T> collection, Comparison<T> comparison)
        where T : IComparable<T>
    {
        var result = collection.ToList();
        result.Sort(comparison);
        return result;
    }
}