using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NoPlan.Infrastructure.Data;
using NoPlan.Infrastructure.Observability;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    private static readonly string[] SqlServerHealthCheckTags = { "db", "sql" };

    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services
            .AddHealthChecks()
            .AddSqlServer(
                provider => provider.GetRequiredService<IConfiguration>().GetConnectionString("Default")!,
                failureStatus: HealthStatus.Unhealthy,
                name: "SQL Server",
                timeout: TimeSpan.FromSeconds(10),
                tags: SqlServerHealthCheckTags);

        builder.Services.AddDbContext<PlannerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")!));
        return builder.AddOpenTelemetry();
    }
}
