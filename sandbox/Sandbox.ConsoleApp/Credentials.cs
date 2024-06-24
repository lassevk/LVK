using LVK.Data.MongoDb;

using MongoDB.Bson;

namespace Sandbox.ConsoleApp;

// [MongoDbCollection("Credentials<T>")]
public class Credentials<T> : MongoDbDocument
    where T : class
{
    public required string Identifier { get; init; }
    public required T Data { get; init; }
    public ObjectId? ParentId { get; init; }
}