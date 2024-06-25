using System.Globalization;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace LVK.Data.MongoDb.Serializers;

internal class TimeOnlySerializer : SerializerBase<TimeOnly>
{
    public override TimeOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        BsonType type = context.Reader.GetCurrentBsonType();
        switch (type)
        {
            case BsonType.String:
                return TimeOnly.ParseExact(context.Reader.ReadString(), ["HH:mm:ss.fff", "HH:mm:ss", "HH:mm"], CultureInfo.InvariantCulture);

            default:
                throw new NotSupportedException($"Cannot convert a {type} into a DateOnly value");
        }
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TimeOnly value)
    {
        if (value.Millisecond > 0)
            context.Writer.WriteString(value.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture));
        else if (value.Second > 0)
            context.Writer.WriteString(value.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
        else
            context.Writer.WriteString(value.ToString("HH:mm", CultureInfo.InvariantCulture));
    }
}