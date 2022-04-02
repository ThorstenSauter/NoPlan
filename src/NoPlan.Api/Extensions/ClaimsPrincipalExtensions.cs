using System.Security.Claims;
using Microsoft.Identity.Web;

namespace NoPlan.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetId(this ClaimsPrincipal user) =>
        Guid.TryParse(user.GetObjectId(), out var id)
            ? id
            : default;
}
