namespace NoPlan.Contracts.Responses.V1.ToDos;

public sealed record TagResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime AssignedAt { get; set; }
}
