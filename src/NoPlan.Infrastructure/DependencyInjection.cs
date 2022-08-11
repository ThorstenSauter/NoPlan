﻿using NoPlan.Infrastructure.Data;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddSqlServer(
                provider => provider.GetRequiredService<IConfiguration>().GetConnectionString("Default")!,
                name: "SQL Server",
                timeout: TimeSpan.FromSeconds(15),
                tags: new[] { "db", "sql" });

        return services.AddDbContext<PlannerContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")!));
    }
}
