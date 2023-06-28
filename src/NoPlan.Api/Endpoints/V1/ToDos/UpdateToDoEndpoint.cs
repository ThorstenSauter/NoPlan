using NoPlan.Api.Services;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class UpdateToDoEndpoint(IToDoService toDoService, IDateTimeProvider dateTimeProvider)
    : EndpointWithMapping<UpdateToDoRequest, ToDoResponse, ToDo>
{
    public override void Configure()
    {
        Put("/todos/{Id}");
        Version(1);
        Policies("User");
    }

    public override async Task HandleAsync(UpdateToDoRequest req, CancellationToken ct)
    {
        var updatedToDo = await toDoService.UpdateAsync(MapToEntity(req));
        if (updatedToDo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(MapFromEntity(updatedToDo), cancellation: ct);
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

    public override ToDo MapToEntity(UpdateToDoRequest r)
    {
        ArgumentNullException.ThrowIfNull(r);
        var updateTime = dateTimeProvider.UtcNow();
        return new()
        {
            Id = r.Id,
            Title = r.Title,
            Description = r.Description,
            Tags = r.Tags.Select(request => MapToEntity(request, updateTime)).ToList(),
            CreatedBy = User.GetId()
        };
    }

    private static Tag MapToEntity(UpdateTagRequest r, DateTime updateTime) =>
        new() { Id = r.Id, Name = r.Name, AssignedAt = updateTime };

    private static TagResponse MapFromEntity(Tag e) =>
        new() { Id = e.Id, Name = e.Name, AssignedAt = e.AssignedAt };
}
