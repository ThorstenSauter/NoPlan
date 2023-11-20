using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
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
    private IPublicClientApplication? _publicClientApplication;

    public NoPlanApiFactory() =>
        AuthenticatedClient = new(
            async () =>
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

        builder.ConfigureAppConfiguration(
            configBuilder =>
            {
                Environment.SetEnvironmentVariable(
                    "APPLICATIONINSIGHTS_CONNECTION_STRING",
                    "InstrumentationKey=00000000-0000-0000-0000-000000000000;");
                configBuilder.AddInMemoryCollection(new Dictionary<string, string?> { { "ConnectionStrings:Default", connectionString } });
                configBuilder.AddUserSecrets<NoPlanApiFactory>();
                configBuilder.AddEnvironmentVariables();

                var config = configBuilder.Build();
                config.GetSection(nameof(UserAuthenticationSettings)).Bind(_userAuthenticationSettings);
            });

        builder.ConfigureServices(
            services =>
            {
                services.RemoveAll<DbContextOptions<PlannerContext>>();
                services.AddDbContext<PlannerContext>(options => options.UseSqlServer(connectionString));
            });
    }

    private async Task<AuthenticationResult> AuthenticateAsync()
    {
        _publicClientApplication ??= await CreatePublicClientApplicationAsync();

        var scopes = new[] { _userAuthenticationSettings.DefaultScope };
        var accounts = await _publicClientApplication.GetAccountsAsync();
        var account = accounts.FirstOrDefault(a => a.Username == _userAuthenticationSettings.Username);

        if (account is not null)
        {
            try
            {
                return await _publicClientApplication.AcquireTokenSilent(scopes, account).ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
            }
        }

        return await _publicClientApplication
            .AcquireTokenByUsernamePassword(
                scopes,
                _userAuthenticationSettings.Username,
                _userAuthenticationSettings.Password)
            .ExecuteAsync();
    }

    private async Task AuthenticateClientAsUserAsync(HttpClient client)
    {
        var authenticationResult = await AuthenticateAsync();
        client.DefaultRequestHeaders.Authorization = new("Bearer", authenticationResult.AccessToken);
    }

    private async Task<IPublicClientApplication> CreatePublicClientApplicationAsync()
    {
        var storageCreationProperties = new StorageCreationPropertiesBuilder(TokenCacheConfiguration.CacheFileName, TokenCacheConfiguration.CacheDir)
            .WithLinuxKeyring(
                TokenCacheConfiguration.LinuxKeyRingSchema,
                TokenCacheConfiguration.LinuxKeyRingCollection,
                TokenCacheConfiguration.LinuxKeyRingLabel,
                TokenCacheConfiguration.LinuxKeyRingAttr1,
                TokenCacheConfiguration.LinuxKeyRingAttr2)
            .WithMacKeyChain(
                TokenCacheConfiguration.KeyChainServiceName,
                TokenCacheConfiguration.KeyChainAccountName)
            .Build();

        var publicClientApplication = PublicClientApplicationBuilder.CreateWithApplicationOptions(
            new()
            {
                AadAuthorityAudience = AadAuthorityAudience.AzureAdMyOrg,
                ClientId = _userAuthenticationSettings.ClientId,
                TenantId = _userAuthenticationSettings.TenantId
            }).Build();

        var cacheHelper = await MsalCacheHelper.CreateAsync(storageCreationProperties);
        cacheHelper.RegisterCache(publicClientApplication.UserTokenCache);

        return publicClientApplication;
    }
}
