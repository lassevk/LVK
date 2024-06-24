namespace LVK.Data.MongoDb;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class MongoDbCollectionAttribute : Attribute
{
    public MongoDbCollectionAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}