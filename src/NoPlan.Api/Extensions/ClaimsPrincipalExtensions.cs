using System.Security.Claims;
using Microsoft.Identity.Web;

namespace NoPlan.Api.Extensions;

/// <summary>
///     Static class containing extension methods for the <see cref="ClaimsPrincipal" /> type.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    ///     Retrieves the user identifier from a <see cref="ClaimsPrincipal" />.
    /// </summary>
    /// <param name="user">The user to retrieve the user identifier from.</param>
    /// <returns>The user identifier if it exists in the claims; <c>default(Guid)</c> otherwise.</returns>
    public static Guid GetId(this ClaimsPrincipal user) =>
        Guid.TryParse(user.GetObjectId(), out var id)
            ? id
            : Guid.Empty;
}
