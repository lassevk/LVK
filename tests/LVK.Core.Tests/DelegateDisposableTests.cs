using NSubstitute;

namespace LVK.Core.Tests;

public class DelegateDisposableTests
{
    [Test]
    public void Dispose_NotCalled_DoesNotInvokeAction()
    {
        Action? action = Substitute.For<Action>();

        var disposable = new DelegateDisposable(action);

        action.DidNotReceive().Invoke();
    }

    [Test]
    public void Dispose_CalledOnce_InvokesAction()
    {
        Action? action = Substitute.For<Action>();
        var disposable = new DelegateDisposable(action);

        disposable.Dispose();

        action.Received().Invoke();
    }

    [Test]
    public void Dispose_CalledTwice_OnlyInvokesActionOnce()
    {
        Action? action = Substitute.For<Action>();
        var disposable = new DelegateDisposable(action);

        disposable.Dispose();
        disposable.Dispose();

        action.Received(1).Invoke();
    }
}