using NoPlan.Api.Services;
using NoPlan.Contracts.Requests.ToDos.V1;
using NoPlan.Contracts.Requests.ToDos.V1.Tags;
using NoPlan.Contracts.Responses.ToDos.V1;
using NoPlan.Contracts.Responses.ToDos.V1.Tags;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Features.ToDos;

public class Create : EndpointWithMapping<CreateToDoRequest, ToDoResponse, ToDo>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IToDoService _toDoService;

    public Create(IToDoService toDoService, IDateTimeProvider dateTimeProvider)
    {
        _toDoService = toDoService;
        _dateTimeProvider = dateTimeProvider;
    }

    public override void Configure()
    {
        Post("/todos");
        Version(1);
        Policies("User");
        Description(b => b
            .Accepts<CreateToDoRequest>("application/json")
            .Produces<ToDoResponse>(201, "application/json")
            .WithName("ToDos.Create")
        );

        Summary(s =>
        {
            s.Summary = "Creates a new ToDo entity.";
            s.Description = "Creates a new ToDo entity with the provided data and returns it.";
            s[201] = "Returns the successfully created ToDo entity.";
        });
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
        new() { Name = e.Name, AssignedAt = e.AssignedAt };
}
