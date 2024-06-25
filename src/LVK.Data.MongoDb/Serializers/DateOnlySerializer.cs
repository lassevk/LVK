using System.Globalization;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace LVK.Data.MongoDb.Serializers;

internal class DateOnlySerializer : SerializerBase<DateOnly>
{
    public override DateOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        BsonType type = context.Reader.GetCurrentBsonType();
        switch (type)
        {
            case BsonType.String:
                return DateOnly.ParseExact(context.Reader.ReadString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            default:
                throw new NotSupportedException($"Cannot convert a {type} into a DateOnly value");
        }
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly value)
    {
        context.Writer.WriteString(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }
}