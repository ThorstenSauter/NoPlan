using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using NoPlan.Api.Endpoints.V1.ToDos;
using NoPlan.Api.Tests.Integration.TestBases;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

[UsesVerify]
public sealed class GetToDoEndpointTests : FakeRequestTest
{
    public GetToDoEndpointTests(NoPlanApiFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn200AndToDos_WhenToDoExistsAndUserIsAuthenticated()
    {
        // Arrange
        var (_, createdToDo) =
            await AuthenticatedClientClient.POSTAsync<CreateToDoEndpoint, CreateToDoRequest, ToDoResponse>(CreateRequestFaker.Generate());

        var request = new GetToDoRequest { Id = createdToDo!.Id };

        // Act
        var (response, result) = await AuthenticatedClientClient.GETAsync<GetToDoEndpoint, GetToDoRequest, ToDoResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Verify(result);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn400_WhenRequestIsMalformed()
    {
        // Arrange
        var request = new GetToDoRequest { Id = Guid.Empty };

        // Act
        var (response, result) = await AuthenticatedClientClient.GETAsync<GetToDoEndpoint, GetToDoRequest, ValidationProblemDetails>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await Verify(result);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn404_WhenToDoDoesNotExistAndUserIsAuthenticated()
    {
        // Arrange
        var request = new GetToDoRequest { Id = Guid.NewGuid() };

        // Act
        var (response, result) = await AuthenticatedClientClient.GETAsync<GetToDoEndpoint, GetToDoRequest, ToDoResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var request = new GetToDoRequest { Id = Guid.NewGuid() };

        // Act
        var (response, result) = await AnonymousClient.GETAsync<GetToDoEndpoint, GetToDoRequest, ToDoResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        result.Should().BeNull();
    }
}
