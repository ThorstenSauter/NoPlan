using NoPlan.Api.Services;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class CreateToDoEndpoint : EndpointWithMapping<CreateToDoRequest, ToDoResponse, ToDo>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IToDoService _toDoService;

    public CreateToDoEndpoint(IToDoService toDoService, IDateTimeProvider dateTimeProvider)
    {
        _toDoService = toDoService;
        _dateTimeProvider = dateTimeProvider;
    }

    public override void Configure()
    {
        Post("/todos");
        Version(1);
        Policies("User");
    }

    public override async Task HandleAsync(CreateToDoRequest req, CancellationToken ct)
    {
        var toDo = await _toDoService.CreateAsync(MapToEntity(req));
        await SendCreatedAtAsync("ToDos.Get", new { toDo.Id }, MapFromEntity(toDo), cancellation: ct);
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

    public override ToDo MapToEntity(CreateToDoRequest r)
    {
        var creationTime = _dateTimeProvider.UtcNow();
        return new()
        {
            Title = r.Title,
            Description = r.Description,
            Tags = r.Tags.Select(request => MapToEntity(request, creationTime)).ToList(),
            CreatedAt = creationTime,
            CreatedBy = User.GetId()
        };
    }

    private Tag MapToEntity(CreateTagRequest r, DateTime creationTime) =>
        new() { Name = r.Name, AssignedAt = creationTime };

    private TagResponse MapFromEntity(Tag e) =>
        new() { Id = e.Id, Name = e.Name, AssignedAt = e.AssignedAt };
}
