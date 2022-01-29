using EndpointSamples.Api.Contracts.Responses;
using EndpointSamples.Api.Services;
using FastEndpoints;

namespace EndpointSamples.Api.Features.ToDos;

public class GetAll : EndpointWithoutRequest<ToDosResponse>
{
    public IToDoService ToDoService { get; set; } = null!;

    public override void Configure()
    {
        Get("/todos");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct) =>
        await SendAsync(new() { ToDos = ToDoService.GetAll() }, cancellation: ct);
}