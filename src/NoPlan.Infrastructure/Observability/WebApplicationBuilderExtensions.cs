using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NoPlan.Infrastructure.Observability;

public static class WebApplicationBuilderExtensions
{
    internal static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddOpenTelemetry().UseAzureMonitor(options => options.Credential = new DefaultAzureCredential());
        return webApplicationBuilder;
    }
}
