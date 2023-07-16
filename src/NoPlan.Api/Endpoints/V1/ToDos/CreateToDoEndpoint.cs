using NoPlan.Api.Features.ToDos;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class CreateToDoEndpoint(IToDoService toDoService, TimeProvider clock) : EndpointWithMapping<CreateToDoRequest, ToDoResponse, ToDo>
{
    public override void Configure()
    {
        Post("/todos");
        Version(1);
        Policies(AuthorizationPolicies.Users);
    }

    public override async Task HandleAsync(CreateToDoRequest req, CancellationToken ct)
    {
        var toDo = await toDoService.CreateAsync(MapToEntity(req));
        await SendCreatedAtAsync("ToDos.Get", new { toDo.Id }, MapFromEntity(toDo), cancellation: ct);
    }

    public override ToDoResponse MapFromEntity(ToDo e)
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

    public override ToDo MapToEntity(CreateToDoRequest r)
    {
        ArgumentNullException.ThrowIfNull(r);

        var creationTime = clock.GetUtcNow().DateTime;
        return new()
        {
            Title = r.Title,
            Description = r.Description,
            Tags = r.Tags.Select(request => MapToEntity(request, creationTime)).ToList(),
            CreatedAt = creationTime,
            CreatedBy = User.GetId()
        };
    }

    private static Tag MapToEntity(CreateTagRequest r, DateTime creationTime) =>
        new() { Name = r.Name, AssignedAt = creationTime };

    private static TagResponse MapFromEntity(Tag e) =>
        new() { Id = e.Id, Name = e.Name, AssignedAt = e.AssignedAt };
}
