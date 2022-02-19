namespace NoPlan.Contracts.Responses.ToDos.V1;

public class ToDosResponse
{
    /// <summary>
    ///     The collection of ToDo entities.
    /// </summary>
    public IEnumerable<ToDoResponse?> ToDos { get; init; } = null!;
}
