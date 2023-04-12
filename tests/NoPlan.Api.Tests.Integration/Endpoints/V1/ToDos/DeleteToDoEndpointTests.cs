using Microsoft.AspNetCore.Mvc;
using NoPlan.Api.Tests.Integration.TestBases;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

[UsesVerify]
public sealed class DeleteToDoEndpointTests : FakeRequestTest
{
    public DeleteToDoEndpointTests(NoPlanApiFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn204AndToDo_WhenToDoExistsAndUserIsAuthenticated()
    {
        // Arrange
        var createResponse = await AuthenticatedClientClient.PostAsJsonAsync("/api/v1/todos", CreateRequestFaker.Generate());
        var createdToDo = await createResponse.Content.ReadFromJsonAsync<ToDoResponse>();

        // Act
        var response = await AuthenticatedClientClient.DeleteAsync($"/api/v1/todos/{createdToDo!.Id}");
        var toDo = await response.Content.ReadFromJsonAsync<ToDoResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        toDo.Should().BeEquivalentTo(createdToDo);
        await Verify(toDo);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn400_WhenRequestIsMalformed()
    {
        // Arrange
        // Act
        var response = await AuthenticatedClientClient.DeleteAsync($"/api/v1/todos/{Guid.Empty}");
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await Verify(problemDetails);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn404_WhenToDoDoesNotExistAndUserIsAuthenticated()
    {
        // Arrange
        var toDoId = Guid.NewGuid();

        // Act
        var response = await AuthenticatedClientClient.DeleteAsync($"/api/v1/todos/{toDoId}");
        var body = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        body.Should().BeEmpty();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var toDoId = Guid.NewGuid().ToString();

        // Act
        var response = await AnonymousClient.DeleteAsync($"/api/v1/todos/{toDoId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
