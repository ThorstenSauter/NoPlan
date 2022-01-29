namespace EndpointSamples.Api.Contracts.Responses;

public record ToDoResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}