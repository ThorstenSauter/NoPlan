namespace NoPlan.Infrastructure.Data.Models;

public sealed class Tag
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime AssignedAt { get; set; }
}
