using Microsoft.Extensions.Options;

namespace NoPlan.Api.Options;

/// <summary>
///     Contains configuration values for connecting to Azure App Configuration.
/// </summary>
public sealed class AppConfigurationOptions : IOptionsSectionDefinition
{
    /// <summary>
    ///     The <see cref="Uri" /> of the Azure App Configuration endpoint.
    /// </summary>
    public Uri EndPoint { get; set; } = null!;

    /// <summary>
    ///     The Azure Service Bus namespace.
    /// </summary>
    public string ServiceBusNamespace { get; set; } = null!;

    /// <summary>
    ///     The name of the Azure Service Bus topic receiving App Configuration events.
    /// </summary>
    public string ServiceBusTopicName { get; set; } = null!;

    /// <summary>
    ///     The name of the Azure Service Bus subscription receiving App Configuration events.
    /// </summary>
    public string ServiceBusSubscriptionName { get; set; } = null!;

    /// <inheritdoc />
    public static string SectionName => "AppConfiguration";
}
