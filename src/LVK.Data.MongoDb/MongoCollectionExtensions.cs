using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using MongoDB.Driver;

namespace LVK.Data.MongoDb;

public static class MongoCollectionExtensions
{
    public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(
        this IMongoCollection<T> collection, FilterDefinition<T>? filter = null, FindOptions<T, T>? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (filter == null)
        {
            Expression<Func<T, bool>> def = _ => true;
            filter = def;
        }

        using IAsyncCursor<T>? cursor = await collection.FindAsync(filter, options, cancellationToken);

        while (await cursor.MoveNextAsync(cancellationToken))
        {
            foreach (T document in cursor.Current)
                yield return document;
        }
    }
}