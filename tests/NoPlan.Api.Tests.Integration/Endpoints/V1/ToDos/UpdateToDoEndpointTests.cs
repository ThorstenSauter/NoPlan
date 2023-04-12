﻿using Microsoft.AspNetCore.Mvc;
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
        var createResponse = await AuthenticatedClientClient.PostAsJsonAsync("/api/v1/todos", CreateRequestFaker.Generate());
        var createdToDo = await createResponse.Content.ReadFromJsonAsync<ToDoResponse>();

        var updateRequest = UpdateRequestFaker.Generate();

        // Act
        var response = await AuthenticatedClientClient.PutAsJsonAsync($"/api/v1/todos/{createdToDo!.Id}", updateRequest);
        var toDo = await response.Content.ReadFromJsonAsync<ToDoResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        toDo!.Id.Should().Be(createdToDo.Id);
        await Verify(toDo);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn200AndUpdatedTagsToDos_WhenExistingTagIsUpdatedAndUserIsAuthenticated()
    {
        // Arrange
        var createResponse = await AuthenticatedClientClient.PostAsJsonAsync("/api/v1/todos", CreateRequestFaker.Generate());
        var createdToDo = await createResponse.Content.ReadFromJsonAsync<ToDoResponse>();

        var updateRequest = UpdateRequestFaker.Generate();
        updateRequest.Tags.Clear();
        var updateTagRequests = createdToDo!.Tags.Take(2).Select(t => new UpdateTagRequest { Id = t.Id, Name = t.Name }).ToList();
        updateTagRequests.Add(new() { Id = createdToDo.Tags.Last().Id, Name = "new tag" });
        updateRequest.Tags.AddRange(updateTagRequests);

        // Act
        var response = await AuthenticatedClientClient.PutAsJsonAsync($"/api/v1/todos/{createdToDo.Id}", updateRequest);
        var toDo = await response.Content.ReadFromJsonAsync<ToDoResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        toDo!.Id.Should().Be(createdToDo.Id);
        await Verify(toDo);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturn400_WhenRequestIsMalformed()
    {
        // Arrange
        var createResponse = await AuthenticatedClientClient.PostAsJsonAsync("/api/v1/todos", CreateRequestFaker.Generate());
        var createdToDo = await createResponse.Content.ReadFromJsonAsync<ToDoResponse>();
        var request = new UpdateToDoRequest
        {
            Title = "a", Description = "  ", Tags = new List<UpdateTagRequest> { new() { Name = string.Empty }, new() }
        };

        // Act
        var response = await AuthenticatedClientClient.PutAsJsonAsync($"/api/v1/todos/{createdToDo!.Id}", request);
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
        var request = UpdateRequestFaker.Generate();

        // Act
        var response = await AuthenticatedClientClient.PutAsJsonAsync($"/api/v1/todos/{toDoId}", request);
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
        var response = await AnonymousClient.GetAsync($"/api/v1/todos/{toDoId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
