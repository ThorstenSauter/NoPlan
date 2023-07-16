using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace NoPlan.Infrastructure.Auth;

public static class AuthorizationOptionsExtensions
{
    private const string UserScope = "User";

    public static void AddUserPolicy(this AuthorizationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.AddPolicy(AuthorizationPolicies.Users, builder => builder.RequireScope(UserScope));
    }
}
