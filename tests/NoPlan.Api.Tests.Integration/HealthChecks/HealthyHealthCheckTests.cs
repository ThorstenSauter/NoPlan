namespace NoPlan.Api.Tests.Integration.HealthChecks;

public sealed class HealthyHealthCheckTests(NoPlanApiFactory apiFactory) : IClassFixture<NoPlanApiFactory>
{
    [Fact]
    public async Task ReadinessProbe_ShouldReturn200_WhenAppIsHealthy()
    {
        // Arrange
        var client = apiFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task LivenessProbe_ShouldReturn200_WhenAppIsHealthy()
    {
        // Arrange
        var client = apiFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/health/live");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
