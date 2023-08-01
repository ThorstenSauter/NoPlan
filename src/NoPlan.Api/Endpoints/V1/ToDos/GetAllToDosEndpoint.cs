using Microsoft.AspNetCore.Http.HttpResults;
using NoPlan.Api.Features.ToDos;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class GetAllToDosEndpoint(IToDoService toDoService) : Endpoint<GetAllToDosRequest, Ok<ToDosResponse>>
{
    public override void Configure()
    {
        Get("/todos");
        Version(1);
        Policies(AuthorizationPolicies.Users);
    }

    public override async Task<Ok<ToDosResponse>> ExecuteAsync(GetAllToDosRequest req, CancellationToken ct) =>
        TypedResults.Ok(MapFromEntity(await toDoService.GetAllAsync(User.GetId(), ct)));

    private static ToDosResponse MapFromEntity(IEnumerable<ToDo> e) => new()
    {
        ToDos = e.Select(t =>
            new ToDoResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Tags = t.Tags.Select(ta => new TagResponse { Id = ta.Id, Name = ta.Name, AssignedAt = ta.AssignedAt }),
                CreatedAt = t.CreatedAt
            })
    };
}
