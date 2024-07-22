using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace LVK.Data.MongoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoClient(this IServiceCollection services, IMongoClient client) => services.AddSingleton(client);

    public static IServiceCollection AddMongoClient(this IServiceCollection services, string connectionString)
        => services.AddSingleton<IMongoClient>(serviceProvider => CreateMongoClient(connectionString, serviceProvider));

    public static IServiceCollection AddKeyedMongoClient(this IServiceCollection services, object? serviceKey, IMongoClient client) => services.AddKeyedSingleton(serviceKey, client);

    public static IServiceCollection AddKeyedMongoClient(this IServiceCollection services, object? serviceKey, string? connectionString)
        => services.AddKeyedSingleton<IMongoClient>(serviceKey, (serviceProvider, _) => CreateMongoClient(connectionString, serviceProvider));

    private static MongoClient CreateMongoClient(string? connectionString, IServiceProvider serviceProvider)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.LoggingSettings = new LoggingSettings(serviceProvider.GetRequiredService<ILoggerFactory>());
        return new MongoClient(settings);
    }

    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, IMongoDatabase database) => services.AddSingleton(database);

    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, string databaseName)
        => services.AddSingleton<IMongoDatabase>(serviceProvider => serviceProvider.GetRequiredService<IMongoClient>().GetDatabase(databaseName));

    public static IServiceCollection AddKeyedMongoDatabase(this IServiceCollection services, object? serviceKey, IMongoDatabase database) => services.AddKeyedSingleton(serviceKey, database);

    public static IServiceCollection AddKeyedMongoDatabase(this IServiceCollection services, object? serviceKey, string? databaseName)
        => services.AddKeyedSingleton<IMongoDatabase>(serviceKey, (serviceProvider, key) => serviceProvider.GetRequiredKeyedService<IMongoClient>(key).GetDatabase(databaseName));

    public static IServiceCollection AddMongoCollection<T>(this IServiceCollection services, string collectionName)
        => services.AddSingleton<IMongoCollection<T>>(serviceProvider => serviceProvider.GetRequiredService<IMongoDatabase>().GetCollection<T>(collectionName));

    public static IServiceCollection AddMongoCollection<T>(this IServiceCollection services)
        => services.AddSingleton<IMongoCollection<T>>(serviceProvider => serviceProvider.GetRequiredService<IMongoDatabase>().GetCollection<T>());

    public static IServiceCollection AddKeyedMongoCollection<T>(this IServiceCollection services, object? serviceKey, string collectionName)
        => services.AddKeyedSingleton<IMongoCollection<T>>(serviceKey, (serviceProvider, key) => serviceProvider.GetRequiredKeyedService<IMongoDatabase>(key).GetCollection<T>(collectionName));

    public static IServiceCollection AddKeyedMongoCollection<T>(this IServiceCollection services, object? serviceKey)
        where T : MongoDbDocument
        => services.AddKeyedSingleton<IMongoCollection<T>>(serviceKey, (serviceProvider, key) => serviceProvider.GetRequiredKeyedService<IMongoDatabase>(key).GetCollection<T>());
}