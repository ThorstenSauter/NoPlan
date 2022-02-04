using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Infrastructure.Data;

public class PlannerContext : DbContext
{
    public PlannerContext(DbContextOptions<PlannerContext> options) : base(options)
    {
    }

    public DbSet<ToDo> ToDos { get; set; } = null!;
}
