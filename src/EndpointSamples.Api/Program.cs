using EndpointSamples.Api.Services;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder();
var services = builder.Services;
services.AddFastEndpoints();
services.AddSwaggerDoc(tagIndex: 0);
services.AddSingleton<IToDoService, ToDoService>();

var app = builder.Build();
app.UseAuthorization();
app.UseFastEndpoints(c => { c.RoutingOptions = o => o.Prefix = "api"; });
app.UseOpenApi(); //add this
app.UseSwaggerUi3(s => s.ConfigureDefaults()); //add this
app.Run();