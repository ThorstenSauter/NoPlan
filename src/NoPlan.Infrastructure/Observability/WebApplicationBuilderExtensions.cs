﻿using System.Reflection;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        var isDevelopment = webApplicationBuilder.Environment.IsDevelopment();

        webApplicationBuilder.Services.AddOpenTelemetry().UseAzureMonitor(options => options.Credential = new DefaultAzureCredential());
        webApplicationBuilder.Services
            .ConfigureInstrumentation()
            .ConfigureMetrics(isDevelopment)
            .ConfigureTracing(isDevelopment);

        return webApplicationBuilder.ConfigureLogging(isDevelopment);
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

    private static IServiceCollection ConfigureTracing(this IServiceCollection services, bool isDevelopment)
    {
        var resourceAttributes = new Dictionary<string, object>
        {
            { "service.name", "NoPlan API" },
            { "service.namespace", "NoPlan" },
            { "service.version", Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "unknown" }
        };

        services.ConfigureOpenTelemetryTracerProvider((_, builder) =>
            builder.ConfigureResource(resourceBuilder => resourceBuilder.AddAttributes(resourceAttributes)));

        if (isDevelopment)
        {
            services.ConfigureOpenTelemetryTracerProvider(builder => builder.AddOtlpExporter());
        }

        return services;
    }

    private static IServiceCollection ConfigureMetrics(this IServiceCollection services, bool isDevelopment)
    {
        if (isDevelopment)
        {
            services.ConfigureOpenTelemetryMeterProvider(builder => builder.AddOtlpExporter());
        }

        return services.ConfigureOpenTelemetryMeterProvider((_, meterProviderBuilder) =>
        {
            meterProviderBuilder.AddEventCountersInstrumentation(c =>
                {
                    c.AddEventSources(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http",
                        "System.Net.Sockets",
                        "System.Net.NameResolution",
                        "System.Net.Security");
                })
                .AddProcessInstrumentation()
                .AddRuntimeInstrumentation();
        });
    }

    private static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder webApplicationBuilder, bool isDevelopment)
    {
        if (isDevelopment)
        {
            webApplicationBuilder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());
        }

        return webApplicationBuilder;
    }
}
