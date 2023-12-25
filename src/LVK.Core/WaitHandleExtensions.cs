namespace LVK;

/// <summary>
/// This class implements extensions for <see cref="WaitHandle"/> to allow it to be used as
/// a <see cref="Task"/>, useful for async/await.
/// </summary>
public static class WaitHandleExtensions
{
    /// <summary>
    /// Return a <see cref="Task"/> that represents the specified <see cref="WaitHandle"/>.
    /// </summary>
    /// <param name="handle">
    /// The <see cref="WaitHandle"/> to wrap in a <see cref="Task"/>.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/> that represents the <paramref name="handle"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="handle"/> is <c>null</c>.
    /// </exception>
    public static Task AsTask(this WaitHandle handle) => AsTask(handle, Timeout.InfiniteTimeSpan);

    /// <summary>
    /// Return a <see cref="Task"/> that represents the specified <see cref="WaitHandle"/>.
    /// </summary>
    /// <param name="handle">
    /// The <see cref="WaitHandle"/> to wrap in a <see cref="Task"/>.
    /// </param>
    /// <param name="timeout">
    /// A timeout that will be applied, if the <paramref name="handle"/> takes longer than this to complete,
    /// the task will be cancelled.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/> that represents the <paramref name="handle"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="handle"/> is <c>null</c>.
    /// </exception>
    public static Task AsTask(this WaitHandle handle, TimeSpan timeout)
    {
        ArgumentNullException.ThrowIfNull(handle);

        var tcs = new TaskCompletionSource<object>();
        RegisteredWaitHandle registration = ThreadPool.RegisterWaitForSingleObject(
            handle, (state, timedOut) =>
            {
                var localTcs = (TaskCompletionSource<object>)state!;
                if (timedOut)
                    localTcs.TrySetCanceled();
                else
                    localTcs.TrySetResult(default!);
            }, tcs, timeout, executeOnlyOnce: true);

        tcs.Task.ContinueWith((_, state) => (state as RegisteredWaitHandle)!.Unregister(null), registration, TaskScheduler.Default);
        return tcs.Task;
    }
}