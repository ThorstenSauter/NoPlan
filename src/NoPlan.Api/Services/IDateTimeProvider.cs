namespace NoPlan.Api.Services;

/// <summary>
///     An interface for retrieving the current <see cref="DateTime" />. Mostly used for easier testing.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    ///     Returns a <see cref="DateTime" /> of the current date and time in UTC timezone.
    /// </summary>
    /// <returns>The current date and time in UTC timezone.</returns>
    DateTime UtcNow();
}
