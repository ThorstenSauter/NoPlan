namespace NoPlan.Api.Tests.Integration;

public sealed class AsyncLazy<T>(Func<Task<T>> taskFactory) : Lazy<Task<T>>(() => Task.Factory.StartNew(taskFactory).Unwrap());
