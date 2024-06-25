using LVK.Core.Bootstrapping;
using LVK.Data.MongoDb.Serializers;

using Microsoft.Extensions.Hosting;

using MongoDB.Bson.Serialization;

namespace LVK.Data.MongoDb;

public sealed class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        BsonSerializer.RegisterSerializer(new DateOnlySerializer());
        BsonSerializer.RegisterSerializer(new TimeOnlySerializer());

        bootstrapper.Bootstrap(new LVK.Typed.ModuleBootstrapper());
    }
}