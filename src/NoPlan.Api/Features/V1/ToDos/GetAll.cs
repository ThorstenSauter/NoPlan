using NoPlan.Api.Services;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Features.V1.ToDos;

public sealed class GetAll : EndpointWithMapping<EmptyRequest, ToDosResponse, IEnumerable<ToDo>>
{
    private readonly IToDoService _toDoService;

    public GetAll(IToDoService toDoService) =>
        _toDoService = toDoService;

    public override void Configure()
    {
        Get("/todos");
        Version(1);
        Policies("User");
        Description(b => b
            .Accepts<EmptyRequest>("application/json")
            .Produces<ToDosResponse>(200, "application/json")
            .WithName("ToDos.GetAll")
        );

        Summary(s =>
        {
            s.Summary = "Retrieves all ToDo entities.";
            s.Description = "Retrieves all ToDo entities and returns them.";
            s[200] = "Returns all ToDo entities.";
        });
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct) =>
        await SendAsync(MapFromEntity(await _toDoService.GetAllAsync(User.GetId())), cancellation: ct);

    public override ToDosResponse MapFromEntity(IEnumerable<ToDo> e) => new()
    {
        ToDos = e.Select(t =>
            new ToDoResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Tags = t.Tags.Select(ta => new TagResponse { Name = ta.Name, AssignedAt = ta.AssignedAt }),
                CreatedAt = t.CreatedAt
            })
    };
}
