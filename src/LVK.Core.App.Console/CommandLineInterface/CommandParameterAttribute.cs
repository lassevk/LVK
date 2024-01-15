namespace LVK.Core.App.Console.CommandLineInterface;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class CommandLineOption : Attribute
{
    public CommandLineOption(string name)
    {
        Name = name.TrimStart('-');
    }

    public string Name { get; }
}