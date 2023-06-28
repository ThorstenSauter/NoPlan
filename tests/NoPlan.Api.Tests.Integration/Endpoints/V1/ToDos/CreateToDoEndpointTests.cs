using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using NoPlan.Api.Endpoints.V1.ToDos;
using NoPlan.Api.Tests.Integration.TestBases;
using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Tests.Integration.Endpoints.V1.ToDos;

[UsesVerify]
public sealed class CreateToDoEndpointTests(NoPlanApiFactory factory) : FakeRequestTest(factory)
{
    [Fact]
    public async Task HandleAsync_ShouldReturn201AndToDo_WhenRequestIsValidAndUserIsAuthenticated()
    {
        // Arrange
        var request = CreateRequestFaker.Generate();

        // Act
        var (response, result) = await AuthenticatedClientClient.POSTAsync<CreateToDoEndpoint, CreateToDoRequest, ToDoResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        await Verify(result);
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
        var (response, result) = await AuthenticatedClientClient.POSTAsync<CreateToDoEndpoint, CreateToDoRequest, ValidationProblemDetails>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await Verify(result);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var request = CreateRequestFaker.Generate();

        // Act
        var (response, _) = await AnonymousClient.POSTAsync<CreateToDoEndpoint, CreateToDoRequest, ToDoResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
