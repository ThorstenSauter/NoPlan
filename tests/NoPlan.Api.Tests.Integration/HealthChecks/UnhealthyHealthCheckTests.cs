namespace NoPlan.Api.Tests.Integration.HealthChecks;

public sealed class UnhealthyHealthCheckTests(NoPlanApiFactory apiFactory) : IClassFixture<NoPlanApiFactory>
{
    [Fact]
    public async Task ReadinessProbe_ShouldReturn503_WhenAppIsUnhealthy()
    {
        // Arrange
        var client = apiFactory.CreateClient();
        await apiFactory.ShutdownDatabaseAsync();

        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
    }
}
