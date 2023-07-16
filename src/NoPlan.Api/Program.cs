using System.Diagnostics;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using NoPlan.Api.Features.ToDos;
using NoPlan.Infrastructure.Data;
using NoPlan.Infrastructure.HeathChecks;

var builder = WebApplication.CreateBuilder();
var configuration = builder.Configuration;

builder.AddInfrastructure();
builder.Services
    .AddFastEndpoints(options => options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All)
    .SwaggerDocument(d =>
    {
        d.MaxEndpointVersion = 1;
        d.ShortSchemaNames = true;

        d.DocumentSettings = s =>
        {
            s.DocumentName = "Release v1";
            s.Title = "NoPlan API";
            s.Version = "v1";
        };
    })
    .AddScoped<IToDoService, ToDoService>()
    .AddAuthorization(options => options.AddUserPolicy())
    .AddMicrosoftIdentityWebApiAuthentication(configuration);

var app = builder.Build();

await new MigrationRunner(app.Services).ApplyMigrationsAsync<PlannerContext>();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";

    c.Errors.ResponseBuilder = (failures, context, status) =>
    {
        var problemDetails = new ValidationProblemDetails
        {
            Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1",
            Status = status,
            Extensions = { ["traceId"] = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier }
        };

        foreach (var failure in failures.GroupBy(f => f.PropertyName))
        {
            problemDetails.Errors[failure.Key] = failure.Select(g => g.ErrorMessage).ToArray();
        }

        return problemDetails;
    };

    c.Versioning.Prefix = "v";
    c.Versioning.DefaultVersion = 1;
    c.Versioning.PrependToRoute = true;
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(s => s.ConfigureDefaults());
}

app.MapHealthChecks("/health/ready", new() { ResponseWriter = JsonHealthCheckResponseWriter.WriteResponse });
app.MapHealthChecks("/health/live", new() { Predicate = _ => false, ResponseWriter = JsonHealthCheckResponseWriter.WriteResponse });

await app.RunAsync();
