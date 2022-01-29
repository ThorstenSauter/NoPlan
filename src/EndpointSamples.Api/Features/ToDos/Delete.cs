using EndpointSamples.Api.Contracts.Requests.ToDos.V1;
using EndpointSamples.Api.Contracts.Responses;
using EndpointSamples.Api.Models;
using EndpointSamples.Api.Services;

namespace EndpointSamples.Api.Features.ToDos;

public class Delete : EndpointWithMapping<DeleteToDoRequest, ToDoResponse, ToDo>
{
    public IToDoService ToDoService { get; set; } = null!;

    public override void Configure()
    {
        Delete("/todos/{Id}");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteToDoRequest req, CancellationToken ct)
    {
        var deletedToDo = ToDoService.Delete(req.Id);
        if (deletedToDo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(MapFromEntity(deletedToDo), cancellation: ct);
    }

    public override ToDoResponse MapFromEntity(ToDo e) =>
        new()
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description
        };
}