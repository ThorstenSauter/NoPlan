using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using NoPlan.Api.Services;
using NoPlan.Infrastructure.Data;
using NoPlan.Infrastructure.HeathChecks;

var builder = WebApplication.CreateBuilder();
var configuration = builder.Configuration;

builder.AddInfrastructure();
builder.Services
    .AddFastEndpoints(options => options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All)
    .AddSwaggerDoc(maxEndpointVersion: 1, shortSchemaNames: true, settings: s =>
    {
        s.DocumentName = "Release v1";
        s.Title = "NoPlan API";
        s.Version = "v1";
    })
    .AddScoped<IToDoService, ToDoService>()
    .AddSingleton<IDateTimeProvider, DateTimeProvider>()
    .AddAuthorization(options => options.AddPolicy("User", b => b.RequireScope("User")))
    .AddMicrosoftIdentityWebApiAuthentication(configuration);

var app = builder.Build();
await ApplyMigrationsAsync<PlannerContext>(app);

app.UseFastEndpoints(c =>
{
    c.Errors.ResponseBuilder = (failures, _, status) =>
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
    c.Endpoints.RoutePrefix = "api";
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

static async Task ApplyMigrationsAsync<T>(IHost webApplication)
    where T : DbContext
{
    using var scope = webApplication.Services.CreateScope();
    var plannerContext = scope.ServiceProvider.GetRequiredService<T>();
    await plannerContext.Database.MigrateAsync();
}
