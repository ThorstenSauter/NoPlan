using Microsoft.AspNetCore.Http.HttpResults;
using NoPlan.Api.Features.ToDos;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class GetToDoEndpoint(IToDoService toDoService) : Endpoint<GetToDoRequest, Results<Ok<ToDoResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("/todos/{Id}");
        Version(1);
        Policies(AuthorizationPolicies.Users);
        Description(b => b.WithName("ToDos.Get"));
    }

    public override async Task<Results<Ok<ToDoResponse>, NotFound>> ExecuteAsync(GetToDoRequest req, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(req);

        var todo = await toDoService.GetAsync(req.Id, User.GetId(), ct);
        if (todo is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(MapFromEntity(todo));
    }

    private static ToDoResponse MapFromEntity(ToDo e)
    {
        ArgumentNullException.ThrowIfNull(e);
        return new()
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Tags = e.Tags.Select(ta => new TagResponse { Id = ta.Id, Name = ta.Name, AssignedAt = ta.AssignedAt }),
            CreatedAt = e.CreatedAt
        };
    }
}
