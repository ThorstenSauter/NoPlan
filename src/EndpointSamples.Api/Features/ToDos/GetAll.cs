using EndpointSamples.Api.Services;
using EndpointSamples.Contracts.Responses.ToDos.V1;
using EndpointsSamples.Infrastructure.Data.Models;

namespace EndpointSamples.Api.Features.ToDos;

public class GetAll : EndpointWithMapping<EmptyRequest, ToDosResponse, IEnumerable<ToDo>>
{
    public IToDoService ToDoService { get; set; } = null!;

    public override void Configure()
    {
        Get("/todos");
        Version(1);
        AllowAnonymous();
        Describe(b => b
            .Accepts<EmptyRequest>("application/json")
            .Produces<ToDosResponse>(200, "application/json")
            .WithName("ToDos.GetAll")
        );
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct) =>
        await SendAsync(MapFromEntity(await ToDoService.GetAllAsync()), cancellation: ct);

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