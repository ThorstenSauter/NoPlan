using Microsoft.AspNetCore.Mvc;
using NoPlan.Api.Tests.Integration.Fakers;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

[UsesVerify]
public sealed class GetToDoEndpointTests : FakeRequestTest, IClassFixture<NoPlanApiFactory>
{
    private readonly NoPlanApiFactory _apiFactory;

    public GetToDoEndpointTests(NoPlanApiFactory apiFactory) =>
        _apiFactory = apiFactory;

    [Fact]
    public async Task HandleAsync_ShouldReturn200AndToDos_WhenToDoExistsAndUserIsAuthenticated()
    {
        // Arrange
        var client = _apiFactory.CreateClient();
        await _apiFactory.AuthenticateClientAsUserAsync(client);
        var createResponse = await client.PostAsJsonAsync("/api/v1/todos", CreateRequestFaker.Generate());
        var createdToDo = await createResponse.Content.ReadFromJsonAsync<ToDoResponse>();

        // Act
        var response = await client.GetAsync($"/api/v1/todos/{createdToDo!.Id}");
        var toDo = await response.Content.ReadFromJsonAsync<ToDoResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Verify(toDo);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn400_WhenRequestIsMalformed()
    {
        // Arrange
        var client = _apiFactory.CreateClient();
        await _apiFactory.AuthenticateClientAsUserAsync(client);

        // Act
        var response = await client.GetAsync($"/api/v1/todos/{Guid.Empty}");
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await Verify(problemDetails);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn404_WhenToDoDoesNotExistAndUserIsAuthenticated()
    {
        // Arrange
        var client = _apiFactory.CreateClient();
        await _apiFactory.AuthenticateClientAsUserAsync(client);
        var toDoId = Guid.NewGuid();

        // Act
        var response = await client.GetAsync($"/api/v1/todos/{toDoId}");
        var body = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        body.Should().BeEmpty();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var client = _apiFactory.CreateClient();
        var toDoId = Guid.NewGuid().ToString();

        // Act
        var response = await client.GetAsync($"/api/v1/todos/{toDoId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
