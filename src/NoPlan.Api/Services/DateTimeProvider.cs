namespace NoPlan.Api.Services;

/// <summary>
///     Implements the <see cref="IDateTimeProvider" /> using the existing functionality from <see cref="DateTime" />.
/// </summary>
public sealed class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTime UtcNow() =>
        DateTime.UtcNow;
}
