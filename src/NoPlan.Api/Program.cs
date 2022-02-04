using Azure.Identity;
using FastEndpoints.Swagger;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Identity.Web;
using NoPlan.Api.Services;
using NoPlan.Infrastructure.Data;

var builder = WebApplication.CreateBuilder();
var configuration = builder.Configuration;
builder.Host.ConfigureAppConfiguration(config =>
    config.AddAzureAppConfiguration(options =>
    {
        var isDevelopment = builder.Environment.IsDevelopment();
        var credential = isDevelopment
            ? new DefaultAzureCredential()
            : new(new DefaultAzureCredentialOptions { ManagedIdentityClientId = configuration.GetValue<string>("ManagedIdentityClientId") });

        options.Connect(configuration.GetValue<Uri>("AppConfiguration:Endpoint"), credential);
        options.ConfigureKeyVault(c => c.SetCredential(credential));
        var prefix = isDevelopment ? "dev" : "prod";
        options.Select(KeyFilter.Any, prefix);
    }));

var services = builder.Services;
services
    .AddFastEndpoints()
    .AddSwaggerDoc(maxEndpointVersion: 1, settings: s =>
    {
        s.DocumentName = "Release 1.0";
        s.Title = "ToDos API";
        s.Version = "v1.0";
    })
    .AddInfrastructure(configuration)
    .AddScoped<IToDoService, ToDoService>()
    .AddSingleton<IDateTimeProvider, DateTimeProvider>()
    .AddAuthorization(options => options.AddPolicy("User", b => b.RequireScope("User")))
    .AddMicrosoftIdentityWebApiAuthentication(configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var plannerContext = scope.ServiceProvider.GetRequiredService<PlannerContext>();
    plannerContext.Database.EnsureCreated();
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
