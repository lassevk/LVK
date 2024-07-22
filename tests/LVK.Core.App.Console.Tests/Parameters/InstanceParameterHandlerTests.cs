using LVK.Core.App.Console.CommandLineInterface;
using LVK.Core.App.Console.Parameters;

namespace LVK.Core.App.Console.Tests.Parameters;

public class InstanceParameterHandlerTests
{
    [Test]
    public void InjectParameters_PositionalArguments_AreAdded()
    {
        var instance = new TheParameterizedInstance();

        InstanceParameterHandler.InjectParameters(instance, ["a", "b", "c"]);

        Assert.That(instance.Positional, Is.EqualTo(new[]
        {
            "a", "b", "c",
        }).AsCollection);
    }

    [Test]
    [TestCase(new[]
    {
        "-b", "true",
    }, true)]
    [TestCase(new[]
    {
        "--boolean", "true",
    }, true)]
    [TestCase(new[]
    {
        "-b", "false",
    }, false)]
    [TestCase(new[]
    {
        "--boolean", "false",
    }, false)]
    [TestCase(new[]
    {
        "-b",
    }, true)]
    [TestCase(new[]
    {
        "--boolean",
    }, true)]
    [TestCase(new string[]
    {
    }, false)]
    public void InjectParameters_BooleanProperty_IsSet(string[] arguments, bool expected)
    {
        var instance = new TheParameterizedInstance();

        InstanceParameterHandler.InjectParameters(instance, arguments);

        Assert.That(instance.BooleanProperty, Is.EqualTo(expected));
    }
}

public class TheParameterizedInstance
{
    [PositionalArguments]
    public List<string> Positional { get; } = new();

    [CommandLineOption("b")]
    [CommandLineOption("boolean")]
    public bool BooleanProperty { get; set; }
}