using System.Runtime.CompilerServices;

namespace LVK;

/// <summary>
/// This class implements <see cref="IEqualityComparer{T}"/> for any given <typeparamref name="T"/>
/// by only comparing references, and not values.
/// </summary>
/// <typeparam name="T">
/// The type of objects to compare references for.
/// </typeparam>
public sealed class ObjectReferenceEqualityComparer<T> : IEqualityComparer<T>
    where T : class
{
    /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
    public bool Equals(T? x, T? y) => ReferenceEquals(x, y);

    /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
    public int GetHashCode(T? obj) => obj is null ? 0 : RuntimeHelpers.GetHashCode(obj);
}