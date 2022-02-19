namespace NoPlan.Contracts.Responses.ToDos.V1;

public record ToDoResponse
{
    /// <summary>
    ///     The unique identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     The title.
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    ///     The description.
    /// </summary>
    public string Description { get; init; } = null!;

    /// <summary>
    ///     The time of creation.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
