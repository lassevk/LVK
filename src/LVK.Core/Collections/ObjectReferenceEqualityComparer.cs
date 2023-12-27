using System.Runtime.CompilerServices;

namespace LVK.Core.Collections;

public sealed class ObjectReferenceEqualityComparer<T> : IEqualityComparer<T>
    where T : class
{
    public bool Equals(T? x, T? y) => ReferenceEquals(x, y);
    public int GetHashCode(T? obj) => obj is null ? 0 : RuntimeHelpers.GetHashCode(obj);
}