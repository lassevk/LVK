using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LVK.Data.MongoDb;

public abstract class MongoDbDocument
{
    /// <summary>
    /// Gets or sets the unique Object Id of this document.
    /// </summary>
    /// <remarks>
    /// The value of this property will be provided by the server when a new object is inserted.
    /// </remarks>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
}