using LVK.Data.MongoDb;

using MongoDB.Bson.Serialization.Attributes;

namespace Sandbox.ConsoleApp.Models;

[MongoDbCollection("items")]
public record RootModel : MongoDbDocument
{
    [BsonElement("item")]
    public required Item Item { get; init; }
}