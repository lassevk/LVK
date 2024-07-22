using LVK.Core;
using LVK.Core.App.Console;
using LVK.Core.Results;

namespace Sandbox.ConsoleApp;

public class MainEntrypoint : IMainEntrypoint
{
    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        Result<FileInfo> result = GetFile(@"D:\Temp\test.yml");
        result.Match(fi => Console.WriteLine($"File exists: {fi.FullName}"), error => Console.WriteLine(error));

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