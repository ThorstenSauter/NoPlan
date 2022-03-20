using NoPlan.Infrastructure.Data;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cosmosConnectionString = configuration.GetConnectionString("Default");
        var databaseName = configuration.GetValue<string>("DatabaseName");
        services
            .AddHealthChecks()
            .AddCosmosDb(
                cosmosConnectionString,
                databaseName,
                "cosmosdb",
                timeout: TimeSpan.FromSeconds(15),
                tags: new[] { "db" });

        return services
            .AddDbContext<PlannerContext>(options => options.UseCosmos(cosmosConnectionString, databaseName));
    }
}
