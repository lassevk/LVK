namespace LVK.Core.App.Console.CommandLineInterface;

[AttributeUsage(AttributeTargets.Class)]
public class CommandNameAttribute : Attribute
{
    public CommandNameAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; }
}