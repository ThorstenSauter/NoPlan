namespace NoPlan.Contracts.Responses.V1.ToDos;

public sealed record ToDosResponse
{
    /// <summary>
    ///     The collection of ToDo entities.
    /// </summary>
    public IEnumerable<ToDoResponse?> ToDos { get; init; } = null!;
}
