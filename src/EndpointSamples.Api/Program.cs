using EndpointSamples.Api.Services;
using EndpointsSamples.Infrastructure.Data;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

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
    .AddScoped<IToDoService, ToDoService>()
    .AddAuthorization(options => options.AddPolicy("User", b => b.RequireScope("User")))
    .AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var plannerContext = scope.ServiceProvider.GetRequiredService<PlannerContext>();
    plannerContext.Database.Migrate();
}

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

app.Run();