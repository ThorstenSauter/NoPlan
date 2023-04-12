namespace NoPlan.Api.Tests.Integration;

public sealed class AsyncLazy<T> : Lazy<Task<T>>
{
    public AsyncLazy(Func<Task<T>> taskFactory) :
        base(() => Task.Factory.StartNew(taskFactory).Unwrap())
    {
    }
}
