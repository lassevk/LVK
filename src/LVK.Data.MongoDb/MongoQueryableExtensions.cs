using System.Runtime.CompilerServices;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LVK.Data.MongoDb;

public static class MongoQueryableExtensions
{
    public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IQueryable<T> collection, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using IAsyncCursor<T>? cursor = await collection.ToCursorAsync(cancellationToken);

        while (await cursor.MoveNextAsync(cancellationToken))
        {
            foreach (T document in cursor.Current)
                yield return document;
        }
    }
}