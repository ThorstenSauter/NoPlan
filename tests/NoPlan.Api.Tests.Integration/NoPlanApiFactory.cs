using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NoPlan.Api.Tests.Integration.Authentication;
using NoPlan.Infrastructure.Data;
using Testcontainers.MsSql;

namespace NoPlan.Api.Tests.Integration;

// ReSharper disable once ClassNeverInstantiated.Global
public class NoPlanApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithPassword("ReallyComplicated01!")
        .Build();

    private readonly UserAuthenticationSettings _userAuthenticationSettings = new();
    private AccessToken? _token;

    public NoPlanApiFactory() =>
        AuthenticatedClient = new(async () =>
        {
            var client = CreateClient();
            await AuthenticateClientAsUserAsync(client);
            return client;
        });

    public AsyncLazy<HttpClient> AuthenticatedClient { get; }

    public async Task InitializeAsync() =>
        await _dbContainer.StartAsync();

    public new async Task DisposeAsync() =>
        await _dbContainer.DisposeAsync();

    public async Task ShutdownDatabaseAsync() =>
        await _dbContainer.StopAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging => logging.ClearProviders());

        var connectionString = new SqlConnectionStringBuilder(_dbContainer.GetConnectionString()) { Encrypt = false, InitialCatalog = "noplan" }
            .ToString();

        builder.ConfigureAppConfiguration(configBuilder =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?> { { "ConnectionStrings:Default", connectionString } });
            configBuilder.AddUserSecrets<NoPlanApiFactory>();
            configBuilder.AddEnvironmentVariables();

            var config = configBuilder.Build();
            config.GetSection(nameof(UserAuthenticationSettings)).Bind(_userAuthenticationSettings);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<PlannerContext>>();
            services.AddDbContext<PlannerContext>(options => options.UseSqlServer(connectionString));
        });
    }

    private async Task AuthenticateClientAsUserAsync(HttpClient client)
    {
        if (!_token.HasValue)
        {
            var credential = new UsernamePasswordCredential(_userAuthenticationSettings.Username, _userAuthenticationSettings.Password,
                _userAuthenticationSettings.TenantId, _userAuthenticationSettings.ClientId);

            _token = await credential.GetTokenAsync(new(new[] { _userAuthenticationSettings.DefaultScope }));
        }

        client.DefaultRequestHeaders.Authorization = new("Bearer", _token.Value.Token);
    }
}
