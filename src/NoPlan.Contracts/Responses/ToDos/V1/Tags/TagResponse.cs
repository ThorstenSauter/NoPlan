namespace NoPlan.Contracts.Responses.ToDos.V1.Tags;

public class TagResponse
{
    public string Name { get; set; } = null!;
    public DateTime AssignedAt { get; set; }
}
