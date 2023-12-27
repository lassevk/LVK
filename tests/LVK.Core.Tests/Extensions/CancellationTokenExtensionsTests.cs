using LVK.Core.Extensions;

namespace LVK.Core.Tests.Extensions;

public class CancellationTokenAwaiterTests
{
    [Test]
    public void GetAwaiter_WithPendingCancellationToken_ReturnsAwaiterWithIsCompletedFalse()
    {
        var cts = new CancellationTokenSource();

        var awaiter = new CancellationTokenAwaiter(cts.Token);

        Assert.That(awaiter.IsCompleted, Is.False);
    }

    [Test]
    public void GetAwaiter_WithCancelledCancellationToken_ReturnsAwaiterWithIsCompletedTrue()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();

        var awaiter = new CancellationTokenAwaiter(cts.Token);

        Assert.That(awaiter.IsCompleted, Is.True);
    }
}