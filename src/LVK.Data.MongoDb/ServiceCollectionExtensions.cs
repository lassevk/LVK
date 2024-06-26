using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

namespace LVK.Data.MongoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoClient(this IServiceCollection services, IMongoClient client) => services.AddSingleton<IMongoClient>(client);
    public static IServiceCollection AddMongoClient(this IServiceCollection services, string connectionString) => services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

    public static IServiceCollection AddKeyedMongoClient(this IServiceCollection services, object? serviceKey, IMongoClient client) => services.AddKeyedSingleton<IMongoClient>(serviceKey, client);

    public static IServiceCollection AddKeyedMongoClient(this IServiceCollection services, object? serviceKey, string? connectionString)
        => services.AddKeyedSingleton<IMongoClient>(serviceKey, new MongoClient(connectionString));

    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, IMongoDatabase database) => services.AddSingleton<IMongoDatabase>(database);

    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, string databaseName)
        => services.AddSingleton<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName));

    public static IServiceCollection AddKeyedMongoDatabase(this IServiceCollection services, object? serviceKey, IMongoDatabase database)
        => services.AddKeyedSingleton<IMongoDatabase>(serviceKey, database);

    public static IServiceCollection AddKeyedMongoDatabase(this IServiceCollection services, object? serviceKey, string? databaseName)
        => services.AddKeyedSingleton<IMongoDatabase>(serviceKey, (sp, sk) => sp.GetRequiredKeyedService<IMongoClient>(sk).GetDatabase(databaseName));

    public static IServiceCollection AddMongoCollection<T>(this IServiceCollection services, string collectionName)
        => services.AddSingleton<IMongoCollection<T>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<T>(collectionName));

    public static IServiceCollection AddMongoCollection<T>(this IServiceCollection services)
        => services.AddSingleton<IMongoCollection<T>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<T>());

    public static IServiceCollection AddKeyedMongoCollection<T>(this IServiceCollection services, object? serviceKey, string collectionName)
        => services.AddKeyedSingleton<IMongoCollection<T>>(serviceKey, (sp, sk) => sp.GetRequiredKeyedService<IMongoDatabase>(sk).GetCollection<T>(collectionName));

    public static IServiceCollection AddKeyedMongoCollection<T>(this IServiceCollection services, object? serviceKey)
        where T : MongoDbDocument
        => services.AddKeyedSingleton<IMongoCollection<T>>(serviceKey, (sp, sk) => sp.GetRequiredKeyedService<IMongoDatabase>(sk).GetCollection<T>());
}