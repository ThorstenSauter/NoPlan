using NoPlan.Api.Extensions;
using NoPlan.Api.Services;
using NoPlan.Contracts.Requests.ToDos.V1;
using NoPlan.Contracts.Responses.ToDos.V1;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Features.ToDos;

public class Update : EndpointWithMapping<UpdateToDoRequest, ToDoResponse, ToDo>
{
    private readonly IToDoService _toDoService;

    public Update(IToDoService toDoService) =>
        _toDoService = toDoService;

    public override void Configure()
    {
        Put("/todos/{Id}");
        Version(1);
        Policies("User");
        Describe(b => b
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
        new() { Id = e.Id, Title = e.Title, Description = e.Description, CreatedAt = e.CreatedAt };

    public override ToDo MapToEntity(UpdateToDoRequest r) =>
        new() { Id = r.Id, Title = r.Title, Description = r.Description, CreatedBy = User.GetId() };
}
