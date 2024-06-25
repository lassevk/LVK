using System.Reflection;

using Humanizer;

using LVK.Typed;

using MongoDB.Driver;

namespace LVK.Data.MongoDb;

public static class MongoDatabaseExtensions
{
    public static IMongoCollection<T> GetCollection<T>(this IMongoDatabase database)
    {
        MongoDbCollectionAttribute? attr = typeof(T).GetCustomAttribute<MongoDbCollectionAttribute>();
        string? collectionName = attr?.Name ?? TypeHelper.Instance.NameOf<T>(NameOfTypeOptions.UseShorthandSyntax).Pluralize();

        return database.GetCollection<T>(collectionName);
    }
}