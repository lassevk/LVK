namespace LVK.Core.Extensions;

public static class DictionaryExtensions
{
    public static TValue? GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue?> dictionary, TKey key, TValue? defaultValue)
        where TKey : notnull
        => GetOrAdd(dictionary, key, () => defaultValue);

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> getDefaultValue)
        where TKey : notnull
    {
        Guard.NotNull(dictionary);
        Guard.NotNull(key);

        if (dictionary.TryGetValue(key, out TValue? value))
            return value;

        value = getDefaultValue();
        dictionary[key] = value;
        return value;
    }
}