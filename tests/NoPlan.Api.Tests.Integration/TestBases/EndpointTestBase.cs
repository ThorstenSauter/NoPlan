namespace NoPlan.Api.Tests.Integration.TestBases;

public class EndpointTestBase : IAsyncLifetime, IClassFixture<NoPlanApiFactory>
{
    private readonly NoPlanApiFactory _factory;

    public EndpointTestBase(NoPlanApiFactory factory) =>
        _factory = factory;

    protected HttpClient AuthenticatedClientClient { get; private set; } = null!;

    protected HttpClient AnonymousClient { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        AuthenticatedClientClient = await _factory.AuthenticatedClient.Value;
        AnonymousClient = _factory.CreateClient();
    }

    public Task DisposeAsync() =>
        Task.CompletedTask;
}
