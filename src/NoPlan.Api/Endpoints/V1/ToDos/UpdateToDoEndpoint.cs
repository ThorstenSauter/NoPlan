using Microsoft.AspNetCore.Http.HttpResults;
using NoPlan.Api.Features.ToDos;
using NoPlan.Api.Mappers;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class UpdateToDoEndpoint(IToDoService toDoService, TimeProvider clock)
    : Endpoint<UpdateToDoRequest, Results<Ok<ToDoResponse>, NotFound>>
{
    public override void Configure()
    {
        Put("/todos/{Id}");
        Version(1);
        Policies(AuthorizationPolicies.Users);
    }

    public override async Task<Results<Ok<ToDoResponse>, NotFound>> ExecuteAsync(UpdateToDoRequest req, CancellationToken ct)
    {
        var updatedToDo = await toDoService.UpdateAsync(req.ToEntity(clock.GetUtcNow().DateTime, User.GetId()));
        return updatedToDo is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(updatedToDo.ToResponse());
    }
}
