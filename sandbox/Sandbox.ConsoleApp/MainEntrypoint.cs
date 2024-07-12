using System.Collections;
using System.Reflection;
using System.Text.Json;

using LVK.Core;
using LVK.Core.App.Console;
using LVK.Core.Results;
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
    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        Result<FileInfo> result = GetFile(@"D:\Temp\test.yml");
        result.Match(onSuccess: fi => Console.WriteLine($"File exists: {fi.FullName}"), onFailure: error => Console.WriteLine(error));

        Console.WriteLine("DONE");
        return result.Match(_ => 0, _ => 1);
    }

    private Result<FileInfo> GetFile(string path)
    {
        var fi = new FileInfo(path);
        if (fi.Exists)
            return Result.Success(fi);

        return Result.Failure<FileInfo>(new FileNotFoundError($"File does not exist, '{path}'"));
    }

    private record FileNotFoundError(string Message) : Error;
}