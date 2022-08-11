using NoPlan.Api.Tests.Integration.Fakers;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

[UsesVerify]
public sealed class UpdateToDoEndpointTests : FakeRequestTest, IClassFixture<NoPlanApiFactory>
{
    private readonly NoPlanApiFactory _apiFactory;

    public UpdateToDoEndpointTests(NoPlanApiFactory apiFactory) =>
        _apiFactory = apiFactory;

    [Fact]
    public async Task HandleAsync_ShouldReturn200AndToDos_WhenToDoExistsAndUserIsAuthenticated()
    {
        // Arrange
        var client = _apiFactory.CreateClient();
        await _apiFactory.AuthenticateClientAsUserAsync(client);
        var createResponse = await client.PostAsJsonAsync("/api/v1/todos", CreateRequestFaker.Generate());
        var createdToDo = await createResponse.Content.ReadFromJsonAsync<ToDoResponse>();

        var updateRequest = UpdateRequestFaker.Generate();

        // Act
        var response = await client.PutAsJsonAsync($"/api/v1/todos/{createdToDo!.Id}", updateRequest);
        var toDo = await response.Content.ReadFromJsonAsync<ToDoResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        toDo!.Id.Should().Be(createdToDo.Id);
        await Verify(toDo);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn404_WhenToDoDoesNotExistAndUserIsAuthenticated()
    {
        // Arrange
        var client = _apiFactory.CreateClient();
        await _apiFactory.AuthenticateClientAsUserAsync(client);
        var toDoId = Guid.NewGuid();
        var request = UpdateRequestFaker.Generate();

        // Act
        var response = await client.PutAsJsonAsync($"/api/v1/todos/{toDoId}", request);
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
