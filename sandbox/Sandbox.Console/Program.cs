using System.Diagnostics;

using LVK;

var lazy = new Lazy<string>(() =>
{
    Thread.Sleep(1000);
    return "Test";
});

var sw = Stopwatch.StartNew();
Console.WriteLine($"{sw.ElapsedMilliseconds}: before");
Console.WriteLine($"{sw.ElapsedMilliseconds}: {await lazy}");
Console.WriteLine($"{sw.ElapsedMilliseconds}: {await lazy}");
Console.WriteLine($"{sw.ElapsedMilliseconds}: after");

IEnumerable<string> n = names();
Guard.NotNull(n);
Guard.NotNull(n);

IEnumerable<string> names()
{
    yield return "A";
    yield return "B";
}