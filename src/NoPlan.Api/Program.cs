using Microsoft.Identity.Web;
using NoPlan.Api.Features.ToDos;
using NoPlan.Api.Validation;
using NoPlan.Contracts;
using NoPlan.Infrastructure.Data;
using NoPlan.Infrastructure.HeathChecks;

using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var bootStrapLogger = loggerFactory.CreateLogger<Program>();

try
{
    var builder = WebApplication.CreateBuilder();
    var configuration = builder.Configuration;

    builder.AddInfrastructure();
    builder.Services
        .AddFastEndpoints(
            options =>
            {
                options.SourceGeneratorDiscoveredTypes.AddRange(DiscoveredTypes.All);
                options.SourceGeneratorDiscoveredTypes.AddRange(DiscoveredTypes.All);
            })
        .AddScoped<IToDoService, ToDoService>()
        .AddAuthorization(options => options.AddUserPolicy())
        .AddMicrosoftIdentityWebApiAuthentication(configuration);

    var app = builder.Build();

    await new MigrationRunner(app.Services).ApplyMigrationsAsync<PlannerContext>();

    app.UseFastEndpoints(
        c =>
        {
            c.Endpoints.RoutePrefix = "api";
            c.Errors.ResponseBuilder = ValidationErrors.ResponseBuilder;
            c.Versioning.Prefix = "v";
            c.Versioning.DefaultVersion = 1;
            c.Versioning.PrependToRoute = true;
        });

    app.MapHealthChecks("/health/ready", new() { ResponseWriter = JsonHealthCheckResponseWriter.WriteResponse });
    app.MapHealthChecks("/health/live", new() { Predicate = _ => false, ResponseWriter = JsonHealthCheckResponseWriter.WriteResponse });

    await app.RunAsync();
}
catch (Exception ex)
{
    bootStrapLogger.LogCritical(ex, "The application shut down unexpectedly");
    throw;
}
