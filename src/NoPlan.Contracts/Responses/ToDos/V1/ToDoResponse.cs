using NoPlan.Contracts.Responses.ToDos.V1.Tags;

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
    ///     The list of associated tags.
    /// </summary>
    public IEnumerable<TagResponse> Tags { get; set; } = new List<TagResponse>();

    /// <summary>
    ///     The time of creation.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
