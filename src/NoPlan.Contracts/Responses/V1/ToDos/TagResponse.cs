namespace NoPlan.Contracts.Responses.V1.ToDos;

public sealed record TagResponse
{
    public string Name { get; set; } = null!;
    public DateTime AssignedAt { get; set; }
}
