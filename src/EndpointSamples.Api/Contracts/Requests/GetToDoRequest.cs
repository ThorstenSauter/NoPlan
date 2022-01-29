using Microsoft.AspNetCore.Mvc;

namespace EndpointSamples.Api.Contracts.Requests;

public record GetToDoRequest
{
    public Guid Id { get; init; }
};