using System.Text.Json;
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
    private TokenResponse? _token;

    public async Task InitializeAsync() =>
        await _dbContainer.StartAsync();

    public new async Task DisposeAsync() =>
        await _dbContainer.DisposeAsync();

    public async Task AuthenticateClientAsUserAsync(HttpClient client)
    {
        if (_token is null)
        {
            var tokenClient = new HttpClient();
            var tokenReq = new HttpRequestMessage(HttpMethod.Post, _userAuthenticationSettings.TokenUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "password",
                    ["username"] = _userAuthenticationSettings.Username,
                    ["password"] = _userAuthenticationSettings.Password,
                    ["client_id"] = _userAuthenticationSettings.ClientId,
                    ["client_secret"] = _userAuthenticationSettings.ClientSecret,
                    ["scope"] = _userAuthenticationSettings.DefaultScope
                })
            };

            var res = await tokenClient.SendAsync(tokenReq);
            var json = await res.Content.ReadAsStringAsync();
            _token = JsonSerializer.Deserialize<TokenResponse>(json);
        }

        client.DefaultRequestHeaders.Authorization = new("Bearer", _token!.AccessToken);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging => logging.ClearProviders());

        var connectionString = new SqlConnectionStringBuilder(_dbContainer.GetConnectionString()) { Encrypt = false, InitialCatalog = "noplan" }
            .ToString();

        builder.ConfigureAppConfiguration(configBuilder =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?> { { "ConnectionStrings:Default", connectionString } });
            configBuilder.AddUserSecrets<NoPlanApiFactory>();

            var config = configBuilder.Build();
            config.GetSection(nameof(UserAuthenticationSettings)).Bind(_userAuthenticationSettings);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<PlannerContext>>();
            services.AddDbContext<PlannerContext>(options => options.UseSqlServer(connectionString));
        });
    }
}
