using Microsoft.AspNetCore.Http.HttpResults;
using NoPlan.Api.Features.ToDos;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class UpdateToDoEndpoint(IToDoService toDoService, TimeProvider clock)
    : Endpoint<UpdateToDoRequest, Results<Ok<ToDoResponse>, NotFound>>
{
    public override void Configure()
    {
        Put("/todos/{Id}");
        Version(1);
        Policies(AuthorizationPolicies.Users);
    }

    public override async Task<Results<Ok<ToDoResponse>, NotFound>> ExecuteAsync(UpdateToDoRequest req, CancellationToken ct)
    {
        var updatedToDo = await toDoService.UpdateAsync(MapToEntity(req));
        if (updatedToDo is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(MapFromEntity(updatedToDo));
    }

    private static Tag MapToEntity(UpdateTagRequest r, DateTime updateTime) =>
        new() { Id = r.Id, Name = r.Name, AssignedAt = updateTime };

    private static ToDoResponse MapFromEntity(ToDo e)
    {
        ArgumentNullException.ThrowIfNull(e);

        return new()
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Tags = e.Tags.Select(MapFromEntity),
            CreatedAt = e.CreatedAt
        };
    }

    private static TagResponse MapFromEntity(Tag e) =>
        new() { Id = e.Id, Name = e.Name, AssignedAt = e.AssignedAt };

    private ToDo MapToEntity(UpdateToDoRequest r)
    {
        ArgumentNullException.ThrowIfNull(r);
        var updateTime = clock.GetUtcNow().DateTime;
        return new()
        {
            Id = r.Id,
            Title = r.Title,
            Description = r.Description,
            Tags = r.Tags.Select(request => MapToEntity(request, updateTime)).ToList(),
            CreatedBy = User.GetId()
        };
    }
}
