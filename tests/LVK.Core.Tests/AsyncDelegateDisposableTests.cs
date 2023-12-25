using NSubstitute;

namespace LVK.Core.Tests;

public class AsyncDelegateDisposableTests
{
    [Test]
    public void Dispose_NotCalled_DoesNotInvokeAction()
    {
        Func<ValueTask> action = Substitute.For<Func<ValueTask>>();
        action.Invoke().Returns(ValueTask.CompletedTask);

        var disposable = new AsyncDelegateDisposable(action);

        action.DidNotReceive().Invoke();
    }

    [Test]
    public async Task Dispose_CalledOnce_InvokesAction()
    {
        Func<ValueTask> action = Substitute.For<Func<ValueTask>>();
        action.Invoke().Returns(ValueTask.CompletedTask);
        var disposable = new AsyncDelegateDisposable(action);

        await disposable.DisposeAsync();

        _ = action.Received(1).Invoke();
    }

    [Test]
    public async Task Dispose_CalledTwice_OnlyInvokesActionOnce()
    {
        Func<ValueTask> action = Substitute.For<Func<ValueTask>>();
        action.Invoke().Returns(ValueTask.CompletedTask);
        var disposable = new AsyncDelegateDisposable(action);

        await disposable.DisposeAsync();
        await disposable.DisposeAsync();

        _ = action.Received(1).Invoke();
    }
}