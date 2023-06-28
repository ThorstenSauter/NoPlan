namespace NoPlan.Api.Tests.Integration.TestBases;

public class EndpointTestBase(NoPlanApiFactory factory) : IAsyncLifetime, IClassFixture<NoPlanApiFactory>
{
    protected HttpClient AuthenticatedClientClient { get; private set; } = null!;

    protected HttpClient AnonymousClient { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        AuthenticatedClientClient = await factory.AuthenticatedClient.Value;
        AnonymousClient = factory.CreateClient();
    }

    public Task DisposeAsync() =>
        Task.CompletedTask;
}
