using System.ComponentModel;
using System.Text.Json;

using LVK.Core.App.Console;
using LVK.Core.App.Console.CommandLineInterface;
using LVK.Core.App.Console.Parameters;

namespace Sandbox.ConsoleApp;

[Description("Some random program")]
public class MainEntrypoint : IMainEntrypoint
{
    public Task<int> RunAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine(JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        }));

        return Task.FromResult(0);
    }

    [CommandLineOption("b")]
    [CommandLineOption("boolean")]
    [Description("A boolean property")]
    public bool BoolProperty { get; set; }

    [PositionalArguments]
    public List<string> Positional { get; } = new();
}