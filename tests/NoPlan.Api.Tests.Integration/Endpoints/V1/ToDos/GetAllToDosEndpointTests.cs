using NoPlan.Api.Tests.Integration.TestBases;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

[UsesVerify]
public sealed class GetAllToDosEndpointTests : FakeRequestTest
{
    public GetAllToDosEndpointTests(NoPlanApiFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn200AndToDos_WhenUserIsAuthenticated()
    {
        // Arrange
        foreach (var request in CreateRequestFaker.Generate(3))
        {
            await AuthenticatedClientClient.PostAsJsonAsync("/api/v1/todos", request);
        }

        // Act
        var response = await AuthenticatedClientClient.GetAsync("/api/v1/todos");
        var toDos = await response.Content.ReadFromJsonAsync<ToDosResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await Verify(toDos);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
    {
        // Arrange
        // Act
        var response = await AnonymousClient.GetAsync("/api/v1/todos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
