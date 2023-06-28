using NoPlan.Api.Services;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class DeleteToDoEndpoint(IToDoService toDoService) : EndpointWithMapping<DeleteToDoRequest, ToDoResponse, ToDo>
{
    public override void Configure()
    {
        Delete("/todos/{Id}");
        Version(1);
        Policies("User");
    }

    public override async Task HandleAsync(DeleteToDoRequest req, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(req);

        var deletedToDo = await toDoService.DeleteAsync(req.Id, User.GetId());
        if (deletedToDo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(MapFromEntity(deletedToDo), cancellation: ct);
    }

    public override ToDoResponse MapFromEntity(ToDo e)
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
