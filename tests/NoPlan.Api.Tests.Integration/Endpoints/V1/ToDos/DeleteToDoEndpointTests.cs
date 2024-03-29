﻿using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using NoPlan.Api.Endpoints.V1.ToDos;
using NoPlan.Api.Tests.Integration.TestBases;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

public sealed class DeleteToDoEndpointTests(NoPlanApiFactory factory) : FakeRequestTest(factory)
{
    [Fact]
    public async Task HandleAsync_ShouldReturn204AndToDo_WhenToDoExistsAndUserIsAuthenticated()
    {
        // Arrange
        var (_, createdToDo) =
            await AuthenticatedClientClient.POSTAsync<CreateToDoEndpoint, CreateToDoRequest, ToDoResponse>(CreateRequestFaker.Generate());

        var deleteRequest = new DeleteToDoRequest { Id = createdToDo.Id };

        // Act
        var (response, result) = await AuthenticatedClientClient.DELETEAsync<DeleteToDoEndpoint, DeleteToDoRequest, ToDoResponse>(deleteRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEquivalentTo(createdToDo);
        await Verify(result);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn400_WhenRequestIsMalformed()
    {
        // Arrange
        var request = new DeleteToDoRequest { Id = Guid.Empty };

        // Act
        var (response, result) =
            await AuthenticatedClientClient.DELETEAsync<DeleteToDoEndpoint, DeleteToDoRequest, ValidationProblemDetails>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await Verify(result);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn404_WhenToDoDoesNotExistAndUserIsAuthenticated()
    {
        // Arrange
        var request = new DeleteToDoRequest { Id = Guid.NewGuid() };

        // Act
        var (response, result) = await AuthenticatedClientClient.DELETEAsync<DeleteToDoEndpoint, DeleteToDoRequest, ToDoResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var request = new DeleteToDoRequest { Id = Guid.NewGuid() };

        // Act
        var (response, result) = await AnonymousClient.DELETEAsync<DeleteToDoEndpoint, DeleteToDoRequest, ToDoResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        result.Should().BeNull();
    }
}
