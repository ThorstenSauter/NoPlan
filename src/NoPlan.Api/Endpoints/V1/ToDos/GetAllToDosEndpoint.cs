﻿using Microsoft.AspNetCore.Http.HttpResults;
using NoPlan.Api.Features.ToDos;
using NoPlan.Api.Mappers;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Endpoints.V1.ToDos;

public sealed class GetAllToDosEndpoint(IToDoService toDoService) : Endpoint<EmptyRequest, Ok<ToDosResponse>>
{
    public override void Configure()
    {
        Get("/todos");
        Version(1);
        Policies(AuthorizationPolicies.Users);
    }

    public override async Task<Ok<ToDosResponse>> ExecuteAsync(EmptyRequest req, CancellationToken ct) =>
        TypedResults.Ok((await toDoService.GetAllAsync(User.GetId(), ct)).ToResponse());
}
