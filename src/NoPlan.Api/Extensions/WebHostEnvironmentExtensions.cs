namespace NoPlan.Api.Extensions;

public static class WebHostEnvironmentExtensions
{
    public const string TestEnvironment = "Testing";

    public static bool IsTesting(this IWebHostEnvironment environment) =>
        environment.EnvironmentName == TestEnvironment;
}
