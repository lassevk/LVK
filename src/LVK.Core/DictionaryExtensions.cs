namespace LVK;

/// <summary>
/// This class provided some extension methods for <see cref="Dictionary{TKey,TValue}"/>.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Gets a value from the dictionary. If the key is not present in the dictionary, the
    /// <paramref name="defaultValue"/> will be added, and then returned.
    /// </summary>
    /// <param name="dictionary">
    /// The <see cref="Dictionary{TKey,TValue}"/> on which to operate.
    /// </param>
    /// <param name="key">
    /// The key to get a value for.
    /// </param>
    /// <param name="defaultValue">
    /// The value that will be added if the <paramref name="key"/> is not present in the
    /// dictionary.
    /// </param>
    /// <typeparam name="TKey">
    /// The type of key in the dictionary.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The type of value in the dictionary.
    /// </typeparam>
    /// <returns>
    /// The value of the key if <paramref name="key"/> is present in the dictionary;
    /// otherwise, <paramref name="defaultValue"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dictionary"/> is <c>null</c>.
    /// <para>- or -</para>
    /// <paramref name="key"/> is <c>null</c>.
    /// </exception>
    public static TValue? GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue?> dictionary, TKey key, TValue? defaultValue)
        where TKey : notnull
        => GetOrAdd(dictionary, key, () => defaultValue);

    /// <summary>
    /// Gets a value from the dictionary. If the key is not present in the dictionary, the
    /// <paramref name="getDefaultValue"/> delegate will be called and the result will be added,
    /// and then returned.
    /// </summary>
    /// <param name="dictionary">
    /// The <see cref="Dictionary{TKey,TValue}"/> on which to operate.
    /// </param>
    /// <param name="key">
    /// The key to get a value for.
    /// </param>
    /// <param name="getDefaultValue">
    /// A delegate that will be called in order to obtain the value that will be added
    /// if the <paramref name="key"/> is not present in the dictionary.
    /// </param>
    /// <typeparam name="TKey">
    /// The type of key in the dictionary.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The type of value in the dictionary.
    /// </typeparam>
    /// <returns>
    /// The value of the key if <paramref name="key"/> is present in the dictionary;
    /// otherwise, the result from calling <paramref name="getDefaultValue"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dictionary"/> is <c>null</c>.
    /// <para>- or -</para>
    /// <paramref name="key"/> is <c>null</c>.
    /// </exception>
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