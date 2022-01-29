using EndpointSamples.Api.Services;
using EndpointSamples.Contracts.Requests.ToDos.V1;
using EndpointSamples.Contracts.Responses.ToDos.V1;
using EndpointsSamples.Infrastructure.Data.Models;

namespace EndpointSamples.Api.Features.ToDos;

public class Update : EndpointWithMapping<UpdateToDoRequest, ToDoResponse, ToDo>
{
    public IToDoService ToDoService { get; set; } = null!;

    public override void Configure()
    {
        Put("/todos/{Id}");
        Version(1);
        AllowAnonymous();
        Describe(b => b
            .Accepts<UpdateToDoRequest>("application/json")
            .Produces<ToDoResponse>(200, "application/json")
            .Produces(404)
            .WithName("ToDos.Update")
        );
    }

    public override async Task HandleAsync(UpdateToDoRequest req, CancellationToken ct)
    {
        var updatedToDo = await ToDoService.UpdateAsync(MapToEntity(req));
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
            Description = e.Description
        };

    public override ToDo MapToEntity(UpdateToDoRequest r) =>
        new()
        {
            Id = r.Id,
            Title = r.Title,
            Description = r.Description
        };
}