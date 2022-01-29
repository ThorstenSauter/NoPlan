using EndpointSamples.Api.Models;
using EndpointSamples.Api.Services;
using EndpointSamples.Contracts.Responses.ToDos.V1;

namespace EndpointSamples.Api.Features.ToDos;

public class GetAll : EndpointWithMapping<EmptyRequest, ToDosResponse, IEnumerable<ToDo>>
{
    public IToDoService ToDoService { get; set; } = null!;

    public override void Configure()
    {
        Get("/todos");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct) =>
        await SendAsync(MapFromEntity(ToDoService.GetAll()), cancellation: ct);

    public override ToDosResponse MapFromEntity(IEnumerable<ToDo> e) => new()
    {
        ToDos = e.Select(t => new ToDoResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description
        })
    };
}