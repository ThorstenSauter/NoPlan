using NoPlan.Api.Services;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Features.V1.ToDos;

public class Update : EndpointWithMapping<UpdateToDoRequest, ToDoResponse, ToDo>
{
    private readonly IToDoService _toDoService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public Update(IToDoService toDoService, IDateTimeProvider dateTimeProvider)
    {
        _toDoService = toDoService;
        _dateTimeProvider = dateTimeProvider;
    }

    public override void Configure()
    {
        Put("/todos/{Id}");
        Version(1);
        Policies("User");
        Description(b => b
            .Accepts<UpdateToDoRequest>("application/json")
            .Produces<ToDoResponse>(200, "application/json")
            .ProducesProblem(404)
            .WithName("ToDos.Update")
        );

        Summary(s =>
        {
            s.Summary = "Updates the specified ToDo entity.";
            s.Description = "Updates the ToDo entity with the provided identifier and data and returns the updated entity.";
            s[200] = "Returns the successfully updated ToDo entity.";
            s[404] = "Returned if the specified ToDo entity does not exist.";
        });
    }

    public override async Task HandleAsync(UpdateToDoRequest req, CancellationToken ct)
    {
        var updatedToDo = await _toDoService.UpdateAsync(MapToEntity(req));
        if (updatedToDo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(MapFromEntity(updatedToDo), cancellation: ct);
    }

    public override ToDoResponse MapFromEntity(ToDo e) =>
        new()
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Tags = e.Tags.Select(MapFromEntity),
            CreatedAt = e.CreatedAt
        };

    public override ToDo MapToEntity(UpdateToDoRequest r)
    {
        var updateTime = _dateTimeProvider.UtcNow();
        return new()
        {
            Id = r.Id,
            Title = r.Title,
            Description = r.Description,
            Tags = r.Tags.Select(request => MapToEntity(request, updateTime)).ToList(),
            CreatedBy = User.GetId()
        };
    }

    private Tag MapToEntity(UpdateTagRequest r, DateTime updateTime) =>
        new() { Name = r.Name, AssignedAt = updateTime };

    private TagResponse MapFromEntity(Tag e) =>
        new() { Name = e.Name, AssignedAt = e.AssignedAt };
}
