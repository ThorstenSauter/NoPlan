﻿using FastEndpoints;
using NoPlan.Api.Endpoints.V1.ToDos;
using NoPlan.Api.Tests.Integration.TestBases;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

public sealed class GetAllToDosEndpointTests(NoPlanApiFactory factory) : FakeRequestTest(factory)
{
    [Fact]
    public async Task HandleAsync_ShouldReturn200AndToDos_WhenUserIsAuthenticated()
    {
        // Arrange
        foreach (var createToDoRequest in CreateRequestFaker.Generate(3))
        {
            await AuthenticatedClientClient.POSTAsync<CreateToDoEndpoint, CreateToDoRequest, ToDoResponse>(createToDoRequest);
        }

        var request = new EmptyRequest();

        // Act
        var (response, result) =
            await AuthenticatedClientClient.GETAsync<GetAllToDosEndpoint, EmptyRequest, ToDosResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Verify(result);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var request = new EmptyRequest();

        // Act
        var (response, result) =
            await AnonymousClient.GETAsync<GetAllToDosEndpoint, EmptyRequest, ToDosResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        result.Should().BeNull();
    }
}
