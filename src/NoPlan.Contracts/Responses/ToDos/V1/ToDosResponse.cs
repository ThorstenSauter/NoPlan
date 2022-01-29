namespace NoPlan.Contracts.Responses.ToDos.V1;

public class ToDosResponse
{
    public IEnumerable<ToDoResponse?> ToDos { get; init; } = null!;
}
