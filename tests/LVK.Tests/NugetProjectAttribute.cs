namespace LVK.Tests;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class NugetProjectAttribute : Attribute
{
    public NugetProjectAttribute(string relativePath)
    {
        RelativePath = relativePath;
    }

    public string RelativePath { get; }
}