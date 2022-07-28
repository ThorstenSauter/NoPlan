namespace NoPlan.Infrastructure.Data.Models;

public sealed class ToDo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
}
