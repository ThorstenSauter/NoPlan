using NoPlan.Api.Services;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Features.V1.ToDos;

public class Get : EndpointWithMapping<GetToDoRequest, ToDoResponse, ToDo>
{
    private readonly IToDoService _toDoService;

    public Get(IToDoService toDoService) =>
        _toDoService = toDoService;

    public override void Configure()
    {
        Get("/todos/{Id}");
        Version(1);
        Policies("User");
        Description(b => b
            .Accepts<GetToDoRequest>("application/json")
            .Produces<ToDoResponse>(200, "application/json")
            .ProducesProblem(404)
            .WithName("ToDos.Get")
        );

        Summary(s =>
        {
            s.Summary = "Retrieves the specified ToDo entity.";
            s.Description = "Retrieves the ToDo entity with the provided identifier and returns it.";
            s[200] = "Returns the ToDo entity.";
            s[404] = "Returned if the specified ToDo entity does not exist.";
        });
    }

    public override async Task HandleAsync(GetToDoRequest req, CancellationToken ct)
    {
        var todo = await _toDoService.GetAsync(req.Id, User.GetId());
        if (todo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(MapFromEntity(todo), cancellation: ct);
    }

    public override ToDoResponse MapFromEntity(ToDo e) =>
        new()
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Tags = e.Tags.Select(ta => new TagResponse { Name = ta.Name, AssignedAt = ta.AssignedAt }),
            CreatedAt = e.CreatedAt
        };
}
