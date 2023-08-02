using Microsoft.AspNetCore.Http.HttpResults;
using NoPlan.Api.Features.ToDos;
using NoPlan.Api.Mappers;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class CreateToDoEndpoint(IToDoService toDoService, TimeProvider clock) : Endpoint<CreateToDoRequest, CreatedAtRoute<ToDoResponse>>
{
    public override void Configure()
    {
        Post("/todos");
        Version(1);
        Policies(AuthorizationPolicies.Users);
    }

    public override async Task<CreatedAtRoute<ToDoResponse>> ExecuteAsync(CreateToDoRequest req, CancellationToken ct)
    {
        var toDo = await toDoService.CreateAsync(MapToEntity(req));
        return TypedResults.CreatedAtRoute(toDo.ToResponse(), "ToDos.Get", new { toDo.Id });
    }

    private ToDo MapToEntity(CreateToDoRequest r)
    {
        ArgumentNullException.ThrowIfNull(r);

        var creationTime = clock.GetUtcNow().DateTime;
        return new()
        {
            Title = r.Title,
            Description = r.Description,
            Tags = r.Tags.Select(request => request.ToEntity(creationTime)).ToList(),
            CreatedAt = creationTime,
            CreatedBy = User.GetId()
        };
    }
}
