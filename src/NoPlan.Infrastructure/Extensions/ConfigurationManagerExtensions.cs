using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Protocols;
using NoPlan.Infrastructure.Options;
using NoPlan.Infrastructure.Workers;

namespace NoPlan.Infrastructure.Extensions;

/// <summary>
///     Contains extension methods for <see cref="ConfigurationManager{T}" />.
/// </summary>
public static class ConfigurationManagerExtensions
{
    private const string ServiceBusHealthCheckName = "Service Bus";

    /// <summary>
    ///     Configures all services related to Azure App Configuration.
    /// </summary>
    /// <param name="configuration">The application configuration manager.</param>
    /// <param name="services">The service collection.</param>
    /// <returns>The configuration manager for chaining.</returns>
    public static ConfigurationManager AddAzureAppConfiguration(this ConfigurationManager configuration, IServiceCollection services)
    {
        var appConfigurationOptions = configuration.GetSection(AppConfigurationOptions.SectionName).Get<AppConfigurationOptions>()!;
        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ManagedIdentityClientId = configuration.GetValue<string>("ManagedIdentityClientId")
        });

        configuration.AddAzureAppConfiguration(options =>
        {
            options.Connect(appConfigurationOptions.EndPoint, credential);
            options.ConfigureKeyVault(c => c.SetCredential(credential));
            const string label = "prod";
            options.Select(KeyFilter.Any, label);
            options.ConfigureRefresh(refreshOptions =>
            {
                refreshOptions.SetCacheExpiration(TimeSpan.FromDays(1));
                refreshOptions.Register("Sentinel", label, true);
            });

            services
                .AddAzureAppConfiguration()
                .AddSectionedOptions<AppConfigurationOptions>(configuration)
                .AddSingleton(options.GetRefresher())
                .AddSingleton(new ServiceBusClient(appConfigurationOptions.ServiceBusNamespace, credential))
                .AddHostedService<AppConfigurationUpdatesHandler>()
                .Configure<HealthCheckPublisherOptions>(opt =>
                {
                    opt.Period = TimeSpan.FromMinutes(1);
                    opt.Predicate = registration => registration.Name != ServiceBusHealthCheckName;
                })
                .AddHealthChecks()
                .AddAzureServiceBusSubscription(
                    appConfigurationOptions.ServiceBusNamespace,
                    appConfigurationOptions.ServiceBusTopicName,
                    appConfigurationOptions.ServiceBusSubscriptionName,
                    credential,
                    ServiceBusHealthCheckName,
                    HealthStatus.Degraded,
                    new[] { "app configuration", "service bus" },
                    TimeSpan.FromSeconds(15));
        });

        return configuration;
    }
}
