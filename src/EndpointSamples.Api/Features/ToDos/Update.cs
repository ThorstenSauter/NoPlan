using EndpointSamples.Api.Contracts.Requests.ToDos.V1;
using EndpointSamples.Api.Contracts.Responses;
using EndpointSamples.Api.Models;
using EndpointSamples.Api.Services;

namespace EndpointSamples.Api.Features.ToDos;

public class Update : EndpointWithMapping<UpdateToDoRequest, ToDoResponse, ToDo>
{
    public IToDoService ToDoService { get; set; } = null!;

    public override void Configure()
    {
        Put("/todos/{Id}");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateToDoRequest req, CancellationToken ct)
    {
        var updatedToDo = ToDoService.Update(MapToEntity(req));
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