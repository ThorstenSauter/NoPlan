﻿namespace EndpointSamples.Contracts.Responses.ToDos.V1;

public record ToDoResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
}
