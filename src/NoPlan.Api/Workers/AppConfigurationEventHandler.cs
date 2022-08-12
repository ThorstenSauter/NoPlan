using Azure.Messaging.EventGrid;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration.Extensions;
using Microsoft.Extensions.Options;
using NoPlan.Api.Options;

namespace NoPlan.Api.Workers;

/// <summary>
///     A background worker service that receives events from Azure Service Bus when Azure App Configuration values change.
/// </summary>
public sealed class AppConfigurationEventHandler : BackgroundService
{
    private readonly ILogger<AppConfigurationEventHandler> _logger;
    private readonly AppConfigurationOptions _options;
    private readonly IConfigurationRefresher _refresher;
    private readonly ServiceBusClient _serviceBusClient;

    /// <summary>
    ///     Creates a new instance of <see cref="AppConfigurationEventHandler" />.
    /// </summary>
    /// <param name="refresher">The configuration refresher used to fetch new configuration values.</param>
    /// <param name="serviceBusClient">The service bus client.</param>
    /// <param name="options">The app configuration options.</param>
    /// <param name="logger">The logger.</param>
    public AppConfigurationEventHandler(IConfigurationRefresher refresher, ServiceBusClient serviceBusClient,
        IOptions<AppConfigurationOptions> options, ILogger<AppConfigurationEventHandler> logger)
    {
        _refresher = refresher;
        _serviceBusClient = serviceBusClient;
        _logger = logger;
        _options = options.Value;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var processor = _serviceBusClient.CreateProcessor(_options.ServiceBusTopicName, _options.ServiceBusSubscriptionName,
            new() { AutoCompleteMessages = true, PrefetchCount = 10 });

        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;
        await processor.StartProcessingAsync(stoppingToken);
    }

    private Task ErrorHandler(ProcessErrorEventArgs arg)
    {
        _logger.LogError(arg.Exception, "Encountered an error when receiving App Configuration event");
        return Task.CompletedTask;
    }

    private Task MessageHandler(ProcessMessageEventArgs args)
    {
        var eventGridEvent = EventGridEvent.Parse(args.Message.Body);
        eventGridEvent.TryCreatePushNotification(out var pushNotification);
        _refresher.ProcessPushNotification(pushNotification);
        return Task.CompletedTask;
    }
}
