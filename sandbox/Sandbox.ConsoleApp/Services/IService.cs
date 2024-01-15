namespace Sandbox.ConsoleApp.Services;

public interface IService
{
    string GetName();
}

internal class Service : IService
{
    public string GetName() => "This is a service";
}