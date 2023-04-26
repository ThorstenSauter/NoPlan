using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using NoPlan.Api.Endpoints.V1.ToDos;
using NoPlan.Api.Tests.Integration.TestBases;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

[UsesVerify]
public sealed class UpdateToDoEndpointTests : FakeRequestTest
{
    public UpdateToDoEndpointTests(NoPlanApiFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn200AndToDos_WhenToDoExistsAndUserIsAuthenticated()
    {
        // Arrange
        var (_, createdToDo) =
            await AuthenticatedClientClient.POSTAsync<CreateToDoEndpoint, CreateToDoRequest, ToDoResponse>(CreateRequestFaker.Generate());

        var updateRequest = UpdateRequestFaker.Generate() with { Id = createdToDo!.Id };

        // Act
        var (response, result) = await AuthenticatedClientClient.PUTAsync<UpdateToDoEndpoint, UpdateToDoRequest, ToDoResponse>(updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.Id.Should().Be(createdToDo.Id);
        await Verify(result);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn200AndUpdatedTagsToDos_WhenExistingTagIsUpdatedAndUserIsAuthenticated()
    {
        // Arrange
        var (_, createdToDo) =
            await AuthenticatedClientClient.POSTAsync<CreateToDoEndpoint, CreateToDoRequest, ToDoResponse>(CreateRequestFaker.Generate());

        var updateRequest = UpdateRequestFaker.Generate() with { Id = createdToDo!.Id };
        updateRequest.Tags.Clear();
        var updateTagRequests = createdToDo.Tags.Take(2).Select(t => new UpdateTagRequest { Id = t.Id, Name = t.Name }).ToList();
        updateTagRequests.Add(new() { Id = createdToDo.Tags.Last().Id, Name = "new tag" });
        updateRequest.Tags.AddRange(updateTagRequests);

        // Act
        var (response, result) = await AuthenticatedClientClient.PUTAsync<UpdateToDoEndpoint, UpdateToDoRequest, ToDoResponse>(updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.Id.Should().Be(createdToDo.Id);
        await Verify(result);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn400_WhenRequestIsMalformed()
    {
        // Arrange
        var (_, createdToDo) =
            await AuthenticatedClientClient.POSTAsync<CreateToDoEndpoint, CreateToDoRequest, ToDoResponse>(CreateRequestFaker.Generate());

        var request = new UpdateToDoRequest
        {
            Id = createdToDo!.Id, Title = "a", Description = "  ", Tags = new() { new() { Name = string.Empty }, new() }
        };

        // Act
        var (response, result) = await AuthenticatedClientClient.PUTAsync<UpdateToDoEndpoint, UpdateToDoRequest, ValidationProblemDetails>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await Verify(result);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn404_WhenToDoDoesNotExistAndUserIsAuthenticated()
    {
        // Arrange
        var request = UpdateRequestFaker.Generate() with { Id = Guid.NewGuid() };

        // Act
        var (response, result) = await AuthenticatedClientClient.PUTAsync<UpdateToDoEndpoint, UpdateToDoRequest, ToDoResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var request = UpdateRequestFaker.Generate() with { Id = Guid.NewGuid() };

        // Act
        var (response, result) = await AnonymousClient.PUTAsync<UpdateToDoEndpoint, UpdateToDoRequest, ToDoResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        result.Should().BeNull();
    }
}
