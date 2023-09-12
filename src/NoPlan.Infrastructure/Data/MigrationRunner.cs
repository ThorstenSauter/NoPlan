using Microsoft.Extensions.DependencyInjection;

namespace NoPlan.Infrastructure.Data;

public sealed class MigrationRunner(IServiceProvider serviceProvider)
{
    public async Task ApplyMigrationsAsync<TContext>()
        where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync();
    }
}
