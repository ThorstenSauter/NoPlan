using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NoPlan.Infrastructure.HeathChecks;

/// <summary>
///     This writer has been adapted from the existing implementation at
///     https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/4f29f80c09e27740e242521a5e07d0b427206f43/src/HealthChecks.UI.Client/UIResponseWriter.cs
///     in order to drop the otherwise unnecessary dependency on the <c>AspNetCore.HealthChecks.UI.Client</c> package.
/// </summary>
public static class JsonHealthCheckResponseWriter
{
    private const string DefaultResponseContentType = "application/json";
    private static readonly byte[] EmptyResponse = "{}"u8.ToArray();
    private static readonly Lazy<JsonSerializerOptions> Options = new(CreateJsonOptions);

    public static async Task WriteResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = DefaultResponseContentType;
        if (report is null)
        {
            await context.Response.BodyWriter.WriteAsync(EmptyResponse);
            return;
        }

        await JsonSerializer.SerializeAsync(context.Response.Body, new MappedHealthReport(report), Options.Value);
    }

    private static JsonSerializerOptions CreateJsonOptions()
    {
        var options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
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

    // ReSharper disable once CollectionNeverQueried.Global
    public Dictionary<string, MappedHealthReportEntry> Entries { get; }
}

internal sealed class MappedHealthReportEntry
{
    public required IReadOnlyDictionary<string, object>? Data { get; init; }
    public required string? Description { get; set; }
    public required TimeSpan Duration { get; init; }
    public string? Exception { get; set; }
    public required HealthStatus Status { get; init; }
    public required IEnumerable<string>? Tags { get; init; }
}
