using EndpointsSamples.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<PlannerContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
}