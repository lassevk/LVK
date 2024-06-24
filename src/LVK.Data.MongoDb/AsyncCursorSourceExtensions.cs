using System.Runtime.CompilerServices;

using MongoDB.Driver;

namespace LVK.Data.MongoDb;

public static class AsyncCursorSourceExtensions
{
    public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IAsyncCursorSource<T> cursorSource, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where T : notnull
    {
        using IAsyncCursor<T>? cursor = await cursorSource.ToCursorAsync(cancellationToken);
        if (cursor is null)
            throw new InvalidOperationException();

        while (await cursor.MoveNextAsync(cancellationToken))
        {
            foreach (T document in cursor.Current)
                yield return document;
        }
    }
}