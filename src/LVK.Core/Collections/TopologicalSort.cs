namespace LVK.Core.Collections;

public static class TopologicalSort
{
    public static IEnumerable<T> Sort<T>(IEnumerable<T> items, IEnumerable<(T item, T dependency)> dependencies)
        where T : IEquatable<T>
        => Sort(items, dependencies, EqualityComparer<T>.Default);

    public static IEnumerable<T> Sort<T>(IEnumerable<T> items, IEnumerable<(T item, T dependency)> dependencies, IEqualityComparer<T> comparer)
        where T : notnull
    {
        var collection = new TopologicalSortCollection<T>(comparer);
        foreach (T item in items)
            collection.Add(item);

        foreach ((T item, T dependency) element in dependencies)
            collection.Add(element.item, element.dependency);

        return collection.Sort();
    }
}

public class TopologicalSortCollection<T>
    where T : notnull
{
    private readonly IEqualityComparer<T> _comparer;
    private readonly HashSet<T> _items;

    private readonly Dictionary<T, HashSet<T>> _comesAfter;
    private readonly Dictionary<T, HashSet<T>> _comesBefore;

    public TopologicalSortCollection(IEqualityComparer<T> comparer)
    {
        _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        _items = new(comparer);

        _comesAfter = new(comparer);
        _comesBefore = new(comparer);
    }

    public void Add(T item)
    {
        _items.Add(item);
    }

    public void Add(T item, T dependsOnItem)
    {
        Add(item);
        Add(dependsOnItem);

        Adjust(_comesAfter, item, dependsOnItem);
        Adjust(_comesBefore, dependsOnItem, item);
    }

    private void Adjust(Dictionary<T, HashSet<T>> dictionary, T key, T value)
    {
        if (!dictionary.TryGetValue(key, out HashSet<T>? values))
        {
            values = new();
            dictionary[key] = values;
        }

        values.Add(value);
    }

    public IEnumerable<T> Sort()
    {
        var nextItems = _items.Where(item => !_comesAfter.ContainsKey(item)).ToList();
        while (nextItems.Count > 0)
        {
            var current = nextItems.ToList();
            nextItems.Clear();

            foreach (T currentItem in current)
            {
                yield return currentItem;

                _items.Remove(currentItem);
                _comesAfter.Remove(currentItem);

                if (!_comesBefore.TryGetValue(currentItem, out HashSet<T>? followingItems))
                    continue;

                foreach (T followingItem in followingItems)
                {
                    HashSet<T> after = _comesAfter[followingItem];
                    after.Remove(currentItem);

                    if (after.Count == 0)
                        nextItems.Add(followingItem);
                }
            }
        }

        if (_items.Count > 0)
            throw new InvalidOperationException("Cyclical dependencies discovered in topological sort, unable to complete sort");
    }
}