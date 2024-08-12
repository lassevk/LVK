using System.Globalization;

using LVK.Core;
using LVK.Core.App.Console;
using LVK.Core.Results;

namespace Sandbox.ConsoleApp;

public class MainEntrypoint : IMainEntrypoint
{
    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        const int progressBars = 10;
        const int scrollWindow = 5;
        var current = new ConsoleLines(progressBars + scrollWindow);

        Task[] tasks = Enumerable.Range(0, progressBars).Select(idx => Walker(s =>
        {
            current.Set(idx, s + $" - Task #{idx + 1}");
        })).ToArray();

        int counter = 0;
        while (!tasks.All(t => t.IsCompleted))
        {
            await Task.Delay(1, stoppingToken);
            current.ScrollUp(progressBars, scrollWindow);
            current.Set(progressBars + scrollWindow - 1, (counter++).ToString(CultureInfo.InvariantCulture));
        }

        await Task.WhenAll(tasks);

        current.Remove();

        return 0;
    }

    private async Task Walker(Action<string> onProgress)
    {
        int delta = Random.Shared.Next(10) + 1;
        var index = 0;
        while (index < 1000)
        {
            index += delta;
            if (index > 1000)
                index = 1000;

            onProgress(ProgressBar.Format(index, 1000));
            await Task.Delay(Random.Shared.Next(15 - delta));
        }

        onProgress(ProgressBar.Format(1000, 1000) + " DONE");
    }
}