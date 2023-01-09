using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NoPlan.Infrastructure.HeathChecks;

internal sealed class MappedHealthReportEntry
{
    public required IReadOnlyDictionary<string, object>? Data { get; init; }

    public required string? Description { get; set; }

    public required TimeSpan Duration { get; init; }

    public string? Exception { get; set; }

    public required HealthStatus Status { get; init; }

    public required IEnumerable<string>? Tags { get; init; }
}
