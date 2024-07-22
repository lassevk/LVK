namespace LVK.Core.Extensions;

public static class WaitHandleExtensions
{
    public static Task AsTask(this WaitHandle handle) => AsTask(handle, Timeout.InfiniteTimeSpan);

    public static Task AsTask(this WaitHandle handle, TimeSpan timeout)
    {
        ArgumentNullException.ThrowIfNull(handle);

        var tcs = new TaskCompletionSource<object>();
        RegisteredWaitHandle registration = ThreadPool.RegisterWaitForSingleObject(handle, (state, timedOut) =>
        {
            var localTcs = (TaskCompletionSource<object>)state!;
            if (timedOut)
                localTcs.TrySetCanceled();
            else
                localTcs.TrySetResult(default!);
        }, tcs, timeout, true);

        tcs.Task.ContinueWith((_, state) => (state as RegisteredWaitHandle)!.Unregister(null), registration, TaskScheduler.Default);
        return tcs.Task;
    }
}