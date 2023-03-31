namespace NoPlan.Api.Tests.Integration.HealthChecks;

public sealed class UnhealthyHealthCheckTests : IClassFixture<NoPlanApiFactory>
{
    private readonly NoPlanApiFactory _apiFactory;

    public UnhealthyHealthCheckTests(NoPlanApiFactory apiFactory) =>
        _apiFactory = apiFactory;

    [Fact]
    public async Task ReadinessProbe_ShouldReturn503_WhenAppIsUnhealthy()
    {
        // Arrange
        var client = _apiFactory.CreateClient();
        await _apiFactory.ShutdownDatabaseAsync();

        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
    }
}
