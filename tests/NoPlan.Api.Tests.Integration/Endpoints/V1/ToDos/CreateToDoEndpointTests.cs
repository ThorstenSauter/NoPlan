using NoPlan.Api.Tests.Integration.Fakers;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

[UsesVerify]
public sealed class CreateToDoEndpointTests : FakeRequestTest, IClassFixture<NoPlanApiFactory>
{
    private readonly NoPlanApiFactory _apiFactory;

    public CreateToDoEndpointTests(NoPlanApiFactory apiFactory) =>
        _apiFactory = apiFactory;

    [Fact]
    public async Task HandleAsync_ShouldReturn201AndToDo_WhenRequestIsValidAndUserIsAuthenticated()
    {
        // Arrange
        var client = _apiFactory.CreateClient();
        await _apiFactory.AuthenticateClientAsUserAsync(client);
        var request = CreateRequestFaker.Generate();

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/todos", request);
        var toDos = await response.Content.ReadFromJsonAsync<ToDoResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        await Verify(toDos);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var client = _apiFactory.CreateClient();
        var request = new CreateToDoRequest
        {
            Title = "Integration test",
            Description = "Create ToDo in integration test",
            Tags = new List<CreateTagRequest> { new() { Name = "integration" }, new() { Name = "test" } }
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/todos", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
