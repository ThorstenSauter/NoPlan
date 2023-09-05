namespace NoPlan.Contracts.Responses.V1.ToDos;

public sealed record ToDosResponse
{
    /// <summary>
    ///     Gets or initializes the collection of <see cref="ToDoResponse" /> entities.
    /// </summary>
    public IEnumerable<ToDoResponse?> ToDos { get; init; } = null!;
}
