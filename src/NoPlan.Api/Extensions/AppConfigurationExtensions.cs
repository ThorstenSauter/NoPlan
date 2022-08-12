using Azure.Identity;
using Azure.Messaging.EventGrid;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration.Extensions;
using NoPlan.Api.Options;

namespace NoPlan.Api.Extensions;

public static class AppConfigurationExtensions
{
    private static IConfigurationRefresher Refresher = null!;

    public static IConfigurationBuilder AddAzureAppConfiguration(this ConfigurationManager configuration)
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

            Refresher = options.GetRefresher();
        });

        var serviceBusClient = new ServiceBusClient(appConfigurationOptions.ServiceBusNamespace, credential);
        var processor = serviceBusClient.CreateProcessor(appConfigurationOptions.ServiceBusTopicName,
            appConfigurationOptions.ServiceBusSubscriptionName, new() { AutoCompleteMessages = true, PrefetchCount = 10 });

        processor.ProcessMessageAsync += MessageHandler;
        return configuration;
    }

    private static Task MessageHandler(ProcessMessageEventArgs args)
    {
        var eventGridEvent = EventGridEvent.Parse(args.Message.Body);
        eventGridEvent.TryCreatePushNotification(out var pushNotification);
        Refresher.ProcessPushNotification(pushNotification);
        return Task.CompletedTask;
    }
}
