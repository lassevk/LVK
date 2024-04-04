using LVK.Core.App.Console;
using LVK.Data.Processing;

namespace Sandbox.ConsoleApp;

public class MainEntrypoint : IMainEntrypoint
{
    private readonly IProcessor<InputComponent, OutputLengthComponent> _processor;

    public MainEntrypoint(IProcessor<InputComponent, OutputLengthComponent> processor)
    {
        _processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        InputComponent[] inputs = [ new InputComponent(Guid.NewGuid()), new InputComponent(Guid.NewGuid()), new InputComponent(Guid.NewGuid()) ];
        Console.WriteLine("start");
        for (var index = 0; index < 1000; index++)
        {
            List<(InputComponent input, OutputLengthComponent? output)> result = await _processor.ProcessAsync(inputs, stoppingToken);
            GC.KeepAlive(result);
        }
        Console.WriteLine("end");

        return 0;
    }
}