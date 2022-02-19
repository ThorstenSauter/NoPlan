using NoPlan.Api.Services;
using NoPlan.Contracts.Requests.ToDos.V1;
using NoPlan.Contracts.Responses.ToDos.V1;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Features.ToDos;

public class Get : EndpointWithMapping<GetToDoRequest, ToDoResponse, ToDo>
{
    public IToDoService ToDoService { get; set; } = null!;

    public override void Configure()
    {
        Get("/todos/{Id}");
        Version(1);
        Policies("User");
        Describe(b => b
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
        var todo = await ToDoService.GetAsync(req.Id);
        if (todo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(MapFromEntity(todo), cancellation: ct);
    }

    public override ToDoResponse MapFromEntity(ToDo e) =>
        new() { Id = e.Id, Title = e.Title, Description = e.Description, CreatedAt = e.CreatedAt };
}
