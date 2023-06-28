using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Infrastructure.Data;

public sealed class PlannerContext(DbContextOptions<PlannerContext> options) : DbContext(options)
{
    public required DbSet<ToDo> ToDos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlannerContext).Assembly);
    }
}
