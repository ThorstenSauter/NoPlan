using NoPlan.Api.Features.ToDos;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class GetToDoEndpoint(IToDoService toDoService) : EndpointWithMapping<GetToDoRequest, ToDoResponse, ToDo>
{
    public override void Configure()
    {
        Get("/todos/{Id}");
        Version(1);
        Policies("User");
        Description(b => b.WithName("ToDos.Get"));
    }

    public override async Task HandleAsync(GetToDoRequest req, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(req);

        var todo = await toDoService.GetAsync(req.Id, User.GetId(), ct);
        if (todo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(MapFromEntity(todo), cancellation: ct);
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
