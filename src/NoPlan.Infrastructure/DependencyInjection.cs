using NoPlan.Infrastructure.Data;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<PlannerContext>(options => options.UseCosmos(
            configuration.GetConnectionString("Default"), configuration.GetValue<string>("DatabaseName")));
}
