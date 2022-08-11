using NoPlan.Api.Tests.Integration.Fakers;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

[UsesVerify]
public sealed class GetAllToDosEndpointTests : FakeRequestTest, IClassFixture<NoPlanApiFactory>
{
    private readonly NoPlanApiFactory _apiFactory;

    public GetAllToDosEndpointTests(NoPlanApiFactory apiFactory) =>
        _apiFactory = apiFactory;

    [Fact]
    public async Task HandleAsync_ShouldReturn200AndToDos_WhenUserIsAuthenticated()
    {
        // Arrange
        var client = _apiFactory.CreateClient();
        await _apiFactory.AuthenticateClientAsUserAsync(client);
        foreach (var request in CreateRequestFaker.Generate(3))
        {
            await client.PostAsJsonAsync("/api/v1/todos", request);
        }

        // Act
        var response = await client.GetAsync("/api/v1/todos");
        var toDos = await response.Content.ReadFromJsonAsync<ToDosResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Verify(toDos);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var client = _apiFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/todos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
