namespace EndpointSamples.Api.Contracts.Responses;

public class ToDosResponse
{
    public IEnumerable<ToDoResponse?> ToDos { get; init; } = null!;
}