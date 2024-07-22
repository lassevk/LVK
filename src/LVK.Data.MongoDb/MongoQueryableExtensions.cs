using System.Runtime.CompilerServices;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LVK.Data.MongoDb;

public static class MongoQueryableExtensions
{
    public static async Task<List<T>> ToListAsync<T>(this IMongoQueryable<T> collection, CancellationToken cancellationToken)
    {
        var result = new List<T>();
        await foreach (T item in collection.AsAsyncEnumerable(cancellationToken))
            result.Add(item);

        return result;
    }

    public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IMongoQueryable<T> collection, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using IAsyncCursor<T>? cursor = await collection.ToCursorAsync(cancellationToken);

        while (await cursor.MoveNextAsync(cancellationToken))
        {
            foreach (T document in cursor.Current)
                yield return document;
        }
    }
}