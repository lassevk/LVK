namespace LVK.Core.Bootstrapping;

public static class App
{
    public static readonly IApplication Instance = new ApplicationImplementation();
}