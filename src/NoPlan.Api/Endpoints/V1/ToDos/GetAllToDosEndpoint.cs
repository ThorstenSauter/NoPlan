using NoPlan.Api.Features.ToDos;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class GetAllToDosEndpoint(IToDoService toDoService) : EndpointWithMapping<GetAllToDosRequest, ToDosResponse, IEnumerable<ToDo>>
{
    public override void Configure()
    {
        Get("/todos");
        Version(1);
        Policies("User");
    }

    public override async Task HandleAsync(GetAllToDosRequest req, CancellationToken ct) =>
        await SendAsync(MapFromEntity(await toDoService.GetAllAsync(User.GetId(), ct)), cancellation: ct);

    public override ToDosResponse MapFromEntity(IEnumerable<ToDo> e) => new()
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
