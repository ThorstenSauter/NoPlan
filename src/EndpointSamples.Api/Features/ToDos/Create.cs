﻿using EndpointSamples.Api.Services;
using EndpointSamples.Contracts.Requests.ToDos.V1;
using EndpointSamples.Contracts.Responses.ToDos.V1;
using EndpointsSamples.Infrastructure.Data.Models;

namespace EndpointSamples.Api.Features.ToDos;

public class Create : EndpointWithMapping<CreateToDoRequest, ToDoResponse, ToDo>
{
    public IToDoService ToDoService { get; set; } = null!;

    public override void Configure()
    {
        Post("/todos");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateToDoRequest req, CancellationToken ct)
    {
        var toDo = ToDoService.Create(MapToEntity(req));
        await SendCreatedAtAsync<Get>(new { toDo.Id }, MapFromEntity(toDo), ct);
    }

    public override ToDoResponse MapFromEntity(ToDo e) =>
        new()
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description
        };

    public override ToDo MapToEntity(CreateToDoRequest r) =>
        new()
        {
            Title = r.Title,
            Description = r.Description
        };
}