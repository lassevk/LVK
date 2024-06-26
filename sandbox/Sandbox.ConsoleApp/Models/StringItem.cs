using MongoDB.Bson.Serialization.Attributes;

namespace Sandbox.ConsoleApp.Models;

[BsonDiscriminator("string")]
public record StringItem : Item
{
    [BsonElement("value")]
    public required string Value { get; init; }
}