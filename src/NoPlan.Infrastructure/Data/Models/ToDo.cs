namespace NoPlan.Infrastructure.Data.Models;

public sealed class ToDo
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

#pragma warning disable CA2227
    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
#pragma warning restore CA2227
}
