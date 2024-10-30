using System.Globalization;

using LVK.Core;
using LVK.Core.App.Console;
using LVK.Core.Results;

namespace Sandbox.ConsoleApp;

public class MainEntrypoint : IMainEntrypoint
{
    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        const int limit = 1000_000;
        var line = new ConsoleLine();
        for (int index = 0; index <= limit; index++)
        {
            line.Set(ProgressBar.Format(index, limit));
            await Task.Yield();
        }

        return 0;
    }
}