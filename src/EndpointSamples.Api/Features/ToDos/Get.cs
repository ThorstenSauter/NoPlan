﻿using EndpointSamples.Api.Services;
using EndpointSamples.Contracts.Requests.ToDos.V1;
using EndpointSamples.Contracts.Responses.ToDos.V1;
using EndpointsSamples.Infrastructure.Data.Models;

namespace EndpointSamples.Api.Features.ToDos;

public class Get : EndpointWithMapping<GetToDoRequest, ToDoResponse, ToDo>
{
    public IToDoService ToDoService { get; set; } = null!;

    public override void Configure()
    {
        Get("/todos/{id}");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetToDoRequest req, CancellationToken ct)
    {
        var todo = ToDoService.Get(req.Id);
        if (todo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(MapFromEntity(todo), cancellation: ct);
    }

    public override ToDoResponse MapFromEntity(ToDo e) =>
        new()
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description
        };
}