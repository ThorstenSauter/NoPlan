using System.Reflection;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Instrumentation.SqlClient;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace NoPlan.Infrastructure.Observability;

public static class WebApplicationBuilderExtensions
{
    internal static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddOpenTelemetry().UseAzureMonitor(options => options.Credential = new DefaultAzureCredential());

        webApplicationBuilder.Services
            .ConfigureInstrumentation()
            .ConfigureMetrics()
            .ConfigureTracing();

        return webApplicationBuilder.ConfigureLogging();
    }

    private static IServiceCollection ConfigureInstrumentation(this IServiceCollection services) =>
        services
            .Configure<AspNetCoreInstrumentationOptions>(options =>
            {
                options.RecordException = true;
                options.EnrichWithHttpRequest = (activity, request) =>
                {
                    activity.SetTag("http.client_ip", request.HttpContext.Connection.RemoteIpAddress);
                    activity.SetTag("enduser.id", request.HttpContext.User.GetObjectId());
                };
            })
            .Configure<HttpClientInstrumentationOptions>(options => options.RecordException = true)
            .Configure<SqlClientInstrumentationOptions>(options =>
            {
                options.RecordException = true;
                options.SetDbStatementForText = true;
            });

    private static IServiceCollection ConfigureTracing(this IServiceCollection services)
    {
        var resourceAttributes = new Dictionary<string, object>
        {
            { "service.name", "NoPlan API" },
            { "service.namespace", "NoPlan" },
            { "service.version", Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "unknown" }
        };

        services.ConfigureOpenTelemetryTracerProvider((sp, builder) =>
            builder.ConfigureResource(resourceBuilder => resourceBuilder.AddAttributes(resourceAttributes)));

        // Workaround due to an issue with the OTLP exporter registering services after the service provider has been built:
        // https://github.com/Azure/azure-sdk-for-net/issues/36339#issuecomment-1552242653
        services.AddOpenTelemetry().WithTracing(builder =>
        {
            builder.AddOtlpExporter();
            builder.SetSampler(new AlwaysOnSampler());
        });

        return services;
    }

    private static IServiceCollection ConfigureMetrics(this IServiceCollection services)
    {
        // Workaround due to an issue with the OTLP exporter registering services after the service provider has been built:
        // https://github.com/Azure/azure-sdk-for-net/issues/36339#issuecomment-1552242653
        services.AddOpenTelemetry().WithMetrics(builder => builder.AddOtlpExporter());

        return services.ConfigureOpenTelemetryMeterProvider((_, meterProviderBuilder) =>
        {
            meterProviderBuilder.AddEventCountersInstrumentation(c =>
                {
                    c.AddEventSources(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft-AspNetCore-Server-Kestrel",
                        "System.Net.Http",
                        "System.Net.Sockets",
                        "System.Net.NameResolution",
                        "System.Net.Security");
                })
                .AddProcessInstrumentation()
                .AddRuntimeInstrumentation();
        });
    }

    private static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

        return webApplicationBuilder;
    }
}
