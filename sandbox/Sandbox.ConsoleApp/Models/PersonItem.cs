using MongoDB.Bson.Serialization.Attributes;

namespace Sandbox.ConsoleApp.Models;

[BsonDiscriminator("person")]
public record PersonItem : Item
{
    [BsonElement("firstName")]
    public required string FirstName { get; init; }

    [BsonIgnoreIfDefault]
    [BsonElement("middleName")]
    public string? MiddleName { get; init; }

    [BsonElement("lastName")]
    public required string LastName { get; init; }
}