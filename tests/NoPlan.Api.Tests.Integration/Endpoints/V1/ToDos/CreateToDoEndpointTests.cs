using Microsoft.AspNetCore.Mvc;
using NoPlan.Api.Tests.Integration.TestBases;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

[UsesVerify]
public sealed class CreateToDoEndpointTests : FakeRequestTest
{
    public CreateToDoEndpointTests(NoPlanApiFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn201AndToDo_WhenRequestIsValidAndUserIsAuthenticated()
    {
        // Arrange
        var request = CreateRequestFaker.Generate();

        // Act
        var response = await AuthenticatedClientClient.PostAsJsonAsync("/api/v1/todos", request);
        var toDos = await response.Content.ReadFromJsonAsync<ToDoResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        await Verify(toDos);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn400_WhenRequestIsMalformed()
    {
        // Arrange
        var request = new CreateToDoRequest
        {
            Title = "a", Description = "  ", Tags = new List<CreateTagRequest> { new() { Name = string.Empty }, new() }
        };

        // Act
        var response = await AuthenticatedClientClient.PostAsJsonAsync("/api/v1/todos", request);
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await Verify(problemDetails);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
    {
        // Arrange
        // Act
        var response = await AnonymousClient.PostAsJsonAsync("/api/v1/todos", CreateRequestFaker.Generate());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
