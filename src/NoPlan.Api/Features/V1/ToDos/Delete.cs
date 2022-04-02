﻿using NoPlan.Api.Services;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Features.V1.ToDos;

public class Delete : EndpointWithMapping<DeleteToDoRequest, ToDoResponse, ToDo>
{
    private readonly IToDoService _toDoService;

    public Delete(IToDoService toDoService) =>
        _toDoService = toDoService;

    public override void Configure()
    {
        Delete("/todos/{Id}");
        Version(1);
        Policies("User");
        Description(b => b
            .Accepts<DeleteToDoRequest>("application/json")
            .Produces<ToDoResponse>(200, "application/json")
            .ProducesProblem(404)
            .WithName("ToDos.Delete")
        );

        Summary(s =>
        {
            s.Summary = "Deletes the specified ToDo entity.";
            s.Description = "Deletes the ToDo entity with the provided identifier and returns the deleted data.";
            s[200] = "Returns the successfully deleted ToDo entity.";
            s[404] = "Returned if the specified ToDo entity did not exist.";
        });
    }

    public override async Task HandleAsync(DeleteToDoRequest req, CancellationToken ct)
    {
        var deletedToDo = await _toDoService.DeleteAsync(req.Id, User.GetId());
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
            Description = e.Description,
            Tags = e.Tags.Select(ta => new TagResponse { Name = ta.Name, AssignedAt = ta.AssignedAt }),
            CreatedAt = e.CreatedAt
        };
}