using Microsoft.AspNetCore.Http.HttpResults;
using NoPlan.Api.Features.ToDos;
using NoPlan.Api.Mappers;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class DeleteToDoEndpoint(IToDoService toDoService) : Endpoint<DeleteToDoRequest, Results<Ok<ToDoResponse>, NotFound>>
{
    public override void Configure()
    {
        Delete("/todos/{Id}");
        Version(1);
        Policies(AuthorizationPolicies.Users);
    }

    public override async Task<Results<Ok<ToDoResponse>, NotFound>> ExecuteAsync(DeleteToDoRequest req, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(req);

        var deletedToDo = await toDoService.DeleteAsync(req.Id, User.GetId());
        return deletedToDo is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(deletedToDo.ToResponse());
    }
}
