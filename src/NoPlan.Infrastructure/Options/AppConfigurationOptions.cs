using Microsoft.Extensions.Options;

namespace NoPlan.Infrastructure.Options;

/// <summary>
///     Contains configuration values for connecting to Azure App Configuration.
/// </summary>
public sealed class AppConfigurationOptions : IOptionsSectionDefinition
{
    /// <inheritdoc cref="IOptionsSectionDefinition.SectionName" />
    public static string SectionName => "AppConfiguration";

    /// <summary>
    ///     Gets or sets the <see cref="Uri" /> of the Azure App Configuration endpoint.
    /// </summary>
    public Uri EndPoint { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the Azure Service Bus namespace.
    /// </summary>
    public string ServiceBusNamespace { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the name of the Azure Service Bus topic receiving App Configuration events.
    /// </summary>
    public string ServiceBusTopicName { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the name of the Azure Service Bus subscription receiving App Configuration events.
    /// </summary>
    public string ServiceBusSubscriptionName { get; set; } = null!;
}
