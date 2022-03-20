using Azure.Identity;
using FastEndpoints.Swagger;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Identity.Web;
using NoPlan.Api.Options;
using NoPlan.Api.Services;
using NoPlan.Infrastructure.Data;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder();
    var configuration = builder.Configuration;
    builder.Host.ConfigureAppConfiguration(config =>
        config.AddAzureAppConfiguration(options =>
        {
            var appConfigurationOptions = configuration.GetSection(AppConfigurationOptions.SectionName).Get<AppConfigurationOptions>();
            var isDevelopment = builder.Environment.IsDevelopment();
            var credential = isDevelopment
                ? new DefaultAzureCredential()
                : new(new DefaultAzureCredentialOptions { ManagedIdentityClientId = configuration.GetValue<string>("ManagedIdentityClientId") });

            options.Connect(appConfigurationOptions.EndPoint, credential);
            options.ConfigureKeyVault(c => c.SetCredential(credential));
            var label = isDevelopment ? "dev" : "prod";
            options.Select(KeyFilter.Any, label);
            options.ConfigureRefresh(refreshOptions =>
            {
                refreshOptions.SetCacheExpiration(TimeSpan.FromSeconds(appConfigurationOptions.RefreshInterval));
                refreshOptions.Register("Sentinel", label, true);
            });
        }));

    builder.Services
        .AddFastEndpoints()
        .AddSwaggerDoc(maxEndpointVersion: 1, settings: s =>
        {
            s.DocumentName = "Release v1";
            s.Title = "NoPlan API";
            s.Version = "v1";
        })
        .AddInfrastructure(configuration)
        .AddAzureAppConfiguration()
        .AddScoped<IToDoService, ToDoService>()
        .AddSingleton<IDateTimeProvider, DateTimeProvider>()
        .AddAuthorization(options => options.AddPolicy("User", b => b.RequireScope("User")))
        .AddMicrosoftIdentityWebApiAuthentication(configuration);

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((ctx, cfg) =>
        cfg.ReadFrom.Configuration(ctx.Configuration)
            .Enrich.WithProperty("ApplicationName", "NoPlan.Api")
            .Enrich.WithProperty("Version", typeof(Program).Assembly.GetName().Version));

    var app = builder.Build();
    EnsureDatabaseCreated(app);

    app.UseAzureAppConfiguration();
    app.UseSerilogRequestLogging(options =>
        options.EnrichDiagnosticContext = (diagnosticsContext, httpContext) =>
        {
            diagnosticsContext.Set("UserId", httpContext.User.GetId());
            diagnosticsContext.Set("RequestId", httpContext.TraceIdentifier);
        });

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseFastEndpoints(c =>
    {
        c.RoutingOptions = o => o.Prefix = "api";
        c.VersioningOptions = o =>
        {
            o.Prefix = "v";
            o.DefaultVersion = 1;
            o.SuffixedVersion = false;
        };
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseOpenApi();
        app.UseSwaggerUi3(s => s.ConfigureDefaults());
    }

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/health/ready", new() { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
        endpoints.MapHealthChecks("/health/live", new() { Predicate = _ => false, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Encountered a fatal error, shutting down");
}
finally
{
    Log.CloseAndFlush();
}

void EnsureDatabaseCreated(IHost webApplication)
{
    using var scope = webApplication.Services.CreateScope();
    var plannerContext = scope.ServiceProvider.GetRequiredService<PlannerContext>();
    plannerContext.Database.EnsureCreated();
}
