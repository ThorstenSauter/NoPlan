using EndpointSamples.Api.Services;
using EndpointsSamples.Infrastructure.Data;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();
var services = builder.Services;
services
    .AddFastEndpoints()
    .AddSwaggerDoc(maxEndpointVersion: 1, settings: s =>
    {
        s.DocumentName = "Release 1.0";
        s.Title = "ToDos API";
        s.Version = "v1.0";
    })
    .AddInfrastructure(builder.Configuration)
    .AddSingleton<IToDoService, ToDoService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var plannerContext = scope.ServiceProvider.GetRequiredService<PlannerContext>();
    plannerContext.Database.Migrate();
}

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

app.UseOpenApi(); //add this
app.UseSwaggerUi3(s => s.ConfigureDefaults()); //add this
app.Run();