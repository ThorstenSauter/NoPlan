using EndpointSamples.Api.Contracts.Requests;
using EndpointSamples.Api.Contracts.Responses;
using EndpointSamples.Api.Services;
using FastEndpoints;

namespace EndpointSamples.Api.Features.ToDos;

public class Get : Endpoint<GetToDoRequest, ToDoResponse>
{
    public IToDoService ToDoService { get; set; }

    public override void Configure()
    {
        Get("/todos/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetToDoRequest req, CancellationToken ct)
    {
        var todo = ToDoService.Get(req.Id);
        if (todo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(todo, cancellation: ct);
    }
}