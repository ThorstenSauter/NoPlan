namespace EndpointSamples.Api.Contracts.Requests.ToDos.V1;

public record GetToDoRequest
{
    public Guid Id { get; init; }
};