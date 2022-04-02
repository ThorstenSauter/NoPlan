using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Infrastructure.Data;

public sealed class PlannerContext : DbContext
{
    public PlannerContext(DbContextOptions<PlannerContext> options) : base(options)
    {
    }

    public DbSet<ToDo> ToDos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlannerContext).Assembly);
}
