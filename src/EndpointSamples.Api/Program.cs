using EndpointSamples.Api.Services;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder();
var services = builder.Services;
services.AddFastEndpoints();
services.AddSwaggerDoc(maxEndpointVersion: 1, settings: s =>
{
    s.DocumentName = "Release 1.0";
    s.Title = "ToDos API";
    s.Version = "v1.0";
});

services.AddSingleton<IToDoService, ToDoService>();

var app = builder.Build();
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