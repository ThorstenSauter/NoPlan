namespace NoPlan.Contracts.Responses.V1.ToDos;

public sealed record ToDoResponse
{
    /// <summary>
    ///     Gets or initializes the unique identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets or initializes the title.
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    ///     Gets or initializes the description.
    /// </summary>
    public string Description { get; init; } = null!;

    /// <summary>
    ///     Gets or sets the list of associated tags.
    /// </summary>
    public IEnumerable<TagResponse> Tags { get; set; } = new List<TagResponse>();

    /// <summary>
    ///     Gets or sets the time of creation.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
