namespace NoPlan.Infrastructure.Data.Models;

public class ToDo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public string ETag { get; set; } = null!;
}
