using System.Collections;
using System.Reflection;
using System.Text.Json;

using LVK.Core.App.Console;
using LVK.Data.BlobStorage;
using LVK.Data.MongoDb;
using LVK.Security.OnePassword;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MongoDB.Driver;

namespace Sandbox.ConsoleApp;

public class MainEntrypoint : IMainEntrypoint
{
    private readonly IMongoCollection<Credentials<UsernamePasswordCredentials>> _credentialsCollection;

    public MainEntrypoint(IMongoCollection<Credentials<UsernamePasswordCredentials>> credentialsCollection)
    {
        _credentialsCollection = credentialsCollection;
    }

    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        await _credentialsCollection.DeleteManyAsync(_ => true, cancellationToken: stoppingToken);

        var root = new Credentials<UsernamePasswordCredentials>
        {
            Identifier = "root",
            Data = new UsernamePasswordCredentials
            {
                Username = "u123", Password = "p456",
            },
        };

        await _credentialsCollection.InsertOneAsync(root, cancellationToken: stoppingToken);

        var child = new Credentials<UsernamePasswordCredentials>
        {
            Identifier = "child",
            Data = new UsernamePasswordCredentials
            {
                Username = "u123", Password = "p456",
            },
            ParentId = root.Id,
        };

        await _credentialsCollection.InsertOneAsync(child, cancellationToken: stoppingToken);

        Console.WriteLine("DONE");
        return 0;
    }
}