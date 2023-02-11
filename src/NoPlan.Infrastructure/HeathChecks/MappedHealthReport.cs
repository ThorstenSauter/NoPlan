using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NoPlan.Infrastructure.HeathChecks;

internal sealed class MappedHealthReport
{
    public MappedHealthReport(HealthReport healthReport)
    {
        ArgumentNullException.ThrowIfNull(healthReport);

        Entries = new();
        TotalDuration = healthReport.TotalDuration;
        Status = healthReport.Status;

        foreach (var (name, reportEntry) in healthReport.Entries)
        {
            var entry = new MappedHealthReportEntry
            {
                Data = reportEntry.Data,
                Description = reportEntry.Description,
                Duration = reportEntry.Duration,
                Status = reportEntry.Status,
                Tags = reportEntry.Tags
            };

            if (reportEntry.Exception is not null)
            {
                var message = reportEntry.Exception?.Message;
                entry.Exception = message;
                entry.Description = reportEntry.Description ?? message;
            }

            Entries.Add(name, entry);
        }
    }

    public HealthStatus Status { get; }

    public TimeSpan TotalDuration { get; }

    public Dictionary<string, MappedHealthReportEntry> Entries { get; }
}
