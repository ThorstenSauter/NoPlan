using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace NoPlan.Infrastructure.Observability;

public static class WebApplicationBuilderExtensions
{
    internal static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder webApplicationBuilder)
    {
        var configuration = webApplicationBuilder.Configuration;
        var openTelemetryCollectorEndpoint = configuration.GetValue<string>("OpenTelemetry:Endpoint")
                                             ?? configuration.GetValue<string>("OTEL_EXPORTER_OTLP_LOGS_ENDPOINT")
                                             ?? configuration.GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT");

        var shouldRegisterOpenTelemetryCollector =
            Uri.TryCreate(openTelemetryCollectorEndpoint, UriKind.Absolute, out var openTelemetryCollectorEndpointUri);

        if (!shouldRegisterOpenTelemetryCollector)
        {
            openTelemetryCollectorEndpointUri = null;
        }

        var applicationInsightsConnectionString = configuration.GetValue<string>("ApplicationInsights:ConnectionString")
                                                  ?? configuration.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING");

        webApplicationBuilder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(ObservabilityResources.ResourceBuilder)
                    .AddExporters(openTelemetryCollectorEndpointUri, applicationInsightsConnectionString)
                    .AddAspNetCoreInstrumentation()
                    .AddEventCountersInstrumentation(c =>
                    {
                        c.AddEventSources(
                            "Microsoft.AspNetCore.Hosting",
                            "Microsoft-AspNetCore-Server-Kestrel",
                            "System.Net.Http",
                            "System.Net.Sockets",
                            "System.Net.NameResolution",
                            "System.Net.Security");
                    })
                    .AddHttpClientInstrumentation()
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(ObservabilityResources.ResourceBuilder)
                    .AddSource(ApiActivitySource.Name)
                    .AddExporters(openTelemetryCollectorEndpointUri, applicationInsightsConnectionString)
                    .SetSampler(new AlwaysOnSampler())
                    .SetErrorStatusOnException()
                    .AddAspNetCoreInstrumentation(options => options.RecordException = true)
                    .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true)
                    .AddHttpClientInstrumentation();
            });

        webApplicationBuilder.Logging
            .ClearProviders()
            .AddOpenTelemetry(logging =>
            {
                logging
                    .SetResourceBuilder(ObservabilityResources.ResourceBuilder)
                    .AddExporters(openTelemetryCollectorEndpointUri, applicationInsightsConnectionString, webApplicationBuilder.Environment);

                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
                logging.ParseStateValues = true;
            });

        return webApplicationBuilder;
    }

    private static OpenTelemetryLoggerOptions AddExporters(this OpenTelemetryLoggerOptions loggerOptions, Uri? openTelemetryCollectorEndpointUri,
        string? applicationInsightsConnectionString, IWebHostEnvironment environment)
    {
        if (openTelemetryCollectorEndpointUri is not null)
        {
            loggerOptions.AddOtlpExporter(exporterOptions => exporterOptions.Endpoint = openTelemetryCollectorEndpointUri);
        }

        if (applicationInsightsConnectionString is not null)
        {
            loggerOptions.AddAzureMonitorLogExporter(options => options.ConnectionString = applicationInsightsConnectionString);
        }

        if (environment.IsDevelopment())
        {
            loggerOptions.AddConsoleExporter();
        }

        return loggerOptions;
    }

    private static MeterProviderBuilder AddExporters(this MeterProviderBuilder metrics, Uri? openTelemetryCollectorEndpointUri,
        string? applicationInsightsConnectionString)
    {
        if (openTelemetryCollectorEndpointUri is not null)
        {
            metrics.AddOtlpExporter(exporterOptions => exporterOptions.Endpoint = openTelemetryCollectorEndpointUri);
        }

        if (applicationInsightsConnectionString is not null)
        {
            metrics.AddAzureMonitorMetricExporter(options => options.ConnectionString = applicationInsightsConnectionString);
        }

        return metrics;
    }

    private static TracerProviderBuilder AddExporters(this TracerProviderBuilder tracing, Uri? openTelemetryCollectorEndpointUri,
        string? applicationInsightsConnectionString)
    {
        if (openTelemetryCollectorEndpointUri is not null)
        {
            tracing.AddOtlpExporter(exporterOptions => exporterOptions.Endpoint = openTelemetryCollectorEndpointUri);
        }

        if (applicationInsightsConnectionString is not null)
        {
            tracing.AddAzureMonitorTraceExporter(options => options.ConnectionString = applicationInsightsConnectionString);
        }

        return tracing;
    }
}
