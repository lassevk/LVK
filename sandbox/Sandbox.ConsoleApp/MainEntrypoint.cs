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
using MongoDB.Driver.Linq;

using Sandbox.ConsoleApp.Models;

namespace Sandbox.ConsoleApp;

public class MainEntrypoint : IMainEntrypoint
{
    private readonly IMongoCollection<RootModel> _rootCollection;

    public MainEntrypoint(IMongoCollection<RootModel> rootCollection)
    {
        _rootCollection = rootCollection;
    }

    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        await _rootCollection.DeleteManyAsync(_ => true, cancellationToken: stoppingToken);

        await _rootCollection.InsertOneAsync(new RootModel
        {
            Item = new StringItem
            {
                Value = "This is a string",
            },
        }, null, stoppingToken);
        await _rootCollection.InsertOneAsync(new RootModel
        {
            Item = new PersonItem
            {
                FirstName = "First",
                MiddleName = "Middle",
                LastName = "Last",
            },
        }, null, stoppingToken);
        await _rootCollection.InsertOneAsync(new RootModel
        {
            Item = new PersonItem
            {
                FirstName = "First",
                LastName = "Middle",
            },
        }, null, stoppingToken);

        await foreach (RootModel? model in _rootCollection.AsQueryable().Where(r => r.Item is PersonItem).AsAsyncEnumerable(stoppingToken))
            Console.WriteLine(model);

        Console.WriteLine("DONE");
        return 0;
    }
}