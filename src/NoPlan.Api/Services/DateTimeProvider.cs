namespace NoPlan.Api.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now() =>
        DateTime.Now;
}
