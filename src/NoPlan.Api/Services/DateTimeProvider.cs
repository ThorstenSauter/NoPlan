namespace NoPlan.Api.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow() =>
        DateTime.UtcNow;
}
