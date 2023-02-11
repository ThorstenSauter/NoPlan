using Azure.Messaging.EventGrid;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoPlan.Infrastructure.Options;

namespace NoPlan.Infrastructure.Workers;

/// <summary>
///     A background worker service that receives events from Azure Service Bus when Azure App Configuration values change.
/// </summary>
public sealed class AppConfigurationUpdatesHandler : BackgroundService
{
    private readonly ILogger<AppConfigurationUpdatesHandler> _logger;
    private readonly AppConfigurationOptions _options;
    private readonly IConfigurationRefresher _refresher;
    private readonly ServiceBusClient _serviceBusClient;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AppConfigurationUpdatesHandler" /> class.
    /// </summary>
    /// <param name="refresher">The configuration refresher used to fetch new configuration values.</param>
    /// <param name="serviceBusClient">The service bus client.</param>
    /// <param name="options">The app configuration options.</param>
    /// <param name="logger">The logger.</param>
    public AppConfigurationUpdatesHandler(IConfigurationRefresher refresher, ServiceBusClient serviceBusClient,
        IOptions<AppConfigurationOptions> options, ILogger<AppConfigurationUpdatesHandler> logger)
    {
        _refresher = refresher;
        _serviceBusClient = serviceBusClient;
        _logger = logger;
        _options = options.Value;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping app configuration handler");
        return base.StopAsync(cancellationToken);
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var processor = _serviceBusClient.CreateProcessor(_options.ServiceBusTopicName, _options.ServiceBusSubscriptionName,
            new() { AutoCompleteMessages = true, PrefetchCount = 10 });

        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;
        _logger.LogInformation("Starting app configuration handler");
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
        _logger.LogInformation("Received app configuration event of type {EventType}", eventGridEvent.EventType);
        eventGridEvent.TryCreatePushNotification(out var pushNotification);
        _refresher.ProcessPushNotification(pushNotification);
        return Task.CompletedTask;
    }
}
