using System.Reflection;
using OpenTelemetry.Resources;

namespace NoPlan.Infrastructure.Observability;

public static class ObservabilityResources
{
    public static readonly ResourceBuilder ResourceBuilder =
        ResourceBuilder.CreateDefault().AddService(
            Assembly.GetEntryAssembly()?.GetName().Name ?? "NoPlan.Api",
            serviceVersion: Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3));
}
