﻿using EndpointSamples.Api.Services;
using EndpointSamples.Contracts.Requests.ToDos.V1;
using EndpointSamples.Contracts.Responses.ToDos.V1;
using EndpointsSamples.Infrastructure.Data.Models;

namespace EndpointSamples.Api.Features.ToDos;

public class Delete : EndpointWithMapping<DeleteToDoRequest, ToDoResponse, ToDo>
{
    public IToDoService ToDoService { get; set; } = null!;

    public override void Configure()
    {
        Delete("/todos/{Id}");
        Version(1);
        Policies("User");
        Describe(b => b
            .Accepts<DeleteToDoRequest>("application/json")
            .Produces<ToDoResponse>(200, "application/json")
            .Produces(404)
            .WithName("ToDos.Delete")
        );
    }

    public override async Task HandleAsync(DeleteToDoRequest req, CancellationToken ct)
    {
        var deletedToDo = await ToDoService.DeleteAsync(req.Id);
        if (deletedToDo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(MapFromEntity(deletedToDo), cancellation: ct);
    }

    public override ToDoResponse MapFromEntity(ToDo e) =>
        new()
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description
        };
}