using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using NoPlan.Api.HeathChecks;
using NoPlan.Api.Services;
using NoPlan.Infrastructure.Data;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder();
    var configuration = builder.Configuration;
    if (builder.Environment.IsProduction())
    {
        configuration.AddAzureAppConfiguration(builder.Services);
    }

    builder.Services
        .AddFastEndpoints(options => options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All)
        .AddSwaggerDoc(maxEndpointVersion: 1, shortSchemaNames: true, settings: s =>
        {
            s.DocumentName = "Release v1";
            s.Title = "NoPlan API";
            s.Version = "v1";
        })
        .AddInfrastructure(configuration)
        .AddApplicationInsightsTelemetry()
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
    await ApplyMigrationsAsync<PlannerContext>(app);

    if (app.Environment.IsProduction())
    {
        app.UseAzureAppConfiguration();
    }

    app.UseSerilogRequestLogging(options =>
        options.EnrichDiagnosticContext = (diagnosticsContext, httpContext) =>
        {
            diagnosticsContext.Set("UserId", httpContext.User.GetId().ToString());
            diagnosticsContext.Set("RequestId", httpContext.TraceIdentifier);
        });

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseFastEndpoints(c =>
    {
        c.ErrorResponseBuilder = (failures, status) =>
        {
            var problemDetails = new ValidationProblemDetails
            {
                Title = "Validation Error",
                Detail = "One or more validation errors occurred",
                Type = $"https://httpstatuses.com/{status}",
                Status = status
            };

            foreach (var failure in failures.GroupBy(f => f.PropertyName))
            {
                problemDetails.Errors[failure.Key] = failure.Select(g => g.ErrorMessage).ToArray();
            }

            return problemDetails;
        };
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

    app.MapHealthChecks("/health/ready", new() { ResponseWriter = JsonHealthCheckResponseWriter.WriteResponse });
    app.MapHealthChecks("/health/live", new() { Predicate = _ => false, ResponseWriter = JsonHealthCheckResponseWriter.WriteResponse });

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Encountered a fatal error, shutting down");
}
finally
{
    Log.CloseAndFlush();
}

async Task ApplyMigrationsAsync<T>(IHost webApplication) where T : DbContext
{
    using var scope = webApplication.Services.CreateScope();
    var plannerContext = scope.ServiceProvider.GetRequiredService<T>();
    await plannerContext.Database.MigrateAsync();
}
