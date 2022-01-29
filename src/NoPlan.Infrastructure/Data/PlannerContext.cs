using EndpointsSamples.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EndpointsSamples.Infrastructure.Data;

public class PlannerContext : DbContext
{
    public DbSet<ToDo> ToDos { get; set; } = null!;

    public PlannerContext(DbContextOptions<PlannerContext> options) : base(options)
    {
    }
}