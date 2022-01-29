using Microsoft.AspNetCore.Mvc;

namespace EndpointSamples.Api.Contracts.Requests.ToDos.V1;

public record DeleteToDoRequest
{
    [FromRoute]
    public Guid Id { get; init; }
}