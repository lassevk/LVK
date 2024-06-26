using LVK.Data.MongoDb;

using MongoDB.Bson.Serialization.Attributes;

namespace Sandbox.ConsoleApp.Models;

[BsonKnownTypes(typeof(StringItem), typeof(PersonItem))]
public abstract record Item : MongoDbDocument
{
}