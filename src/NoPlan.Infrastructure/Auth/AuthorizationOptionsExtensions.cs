using Microsoft.AspNetCore.Authorization;

namespace NoPlan.Infrastructure.Auth;

public static class AuthorizationOptionsExtensions
{
    private const string UserScope = "User";
    private const string UserPolicyName = "User";

    public static void AddUserPolicy(this AuthorizationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.AddPolicy(UserPolicyName, builder => builder.RequireClaim("scp", UserScope));
    }
}
