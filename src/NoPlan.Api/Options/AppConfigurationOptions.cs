using Microsoft.Extensions.Options;

namespace NoPlan.Api.Options;

/// <summary>
///     Contains configuration values for connecting to Azure App Configuration.
/// </summary>
public sealed class AppConfigurationOptions : IOptionsSectionDefinition
{
    private const int DefaultRefreshInterval = 600;

    private int _refreshInterval = DefaultRefreshInterval;

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
        set => _refreshInterval = value <= 0 ? DefaultRefreshInterval : value;
    }

    public static string SectionName => "AppConfiguration";
}
