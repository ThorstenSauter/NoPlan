namespace NoPlan.Api.Options;

/// <summary>
///     Contains configuration values for connecting to Azure App Configuration.
/// </summary>
public sealed class AppConfigurationOptions
{
    /// <summary>
    ///     The <see cref="IConfiguration" /> section name.
    /// </summary>
    public const string SectionName = "AppConfiguration";

    private int _refreshInterval = 300;

    /// <summary>
    ///     The <see cref="Uri" /> of the Azure App Configuration endpoint.
    /// </summary>
    public Uri EndPoint { get; set; } = null!;

    /// <summary>
    ///     The refresh interval in seconds. Defaults to 300 seconds (5 minutes) if no or an illegal value is supplied.
    /// </summary>
    public int RefreshInterval
    {
        get => _refreshInterval;
        set => _refreshInterval = value <= 0 ? 300 : value;
    }
}
