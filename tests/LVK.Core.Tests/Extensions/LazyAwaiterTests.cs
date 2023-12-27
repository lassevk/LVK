using LVK.Core.Extensions;

using NSubstitute;

namespace LVK.Core.Tests;

public class LazyAwaiterTests
{
    [Test]
    public async Task Await_OnLazy_CallsDelegate()
    {
        var called = false;

        string action()
        {
            called = true;
            return "TEST";
        }

        var lazy = new Lazy<string>(action);

        string output = await lazy;

        Assert.That(output, Is.EqualTo("TEST"));
        Assert.That(called, Is.True);
    }

    [Test]
    public async Task Await_CalledTwiceOnLazy_CallsDelegateOnlyOnce()
    {
        var called = 0;

        string action()
        {
            called++;
            return "TEST";
        }

        var lazy = new Lazy<string>(action);

        string output1 = await lazy;
        string output2 = await lazy;

        Assert.That(output1, Is.EqualTo("TEST"));
        Assert.That(output2, Is.EqualTo("TEST"));
        Assert.That(called, Is.EqualTo(1));
    }
}