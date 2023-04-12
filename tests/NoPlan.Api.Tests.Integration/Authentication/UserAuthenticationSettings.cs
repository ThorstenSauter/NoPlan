namespace NoPlan.Api.Tests.Integration.Authentication;

/// <summary>
///     Contains all necessary information for performing the OAuth 2.0 ROPC flow for a user account.
/// </summary>
public sealed class UserAuthenticationSettings
{
    /// <summary>
    ///     Gets or sets the application audience.
    /// </summary>
    public string Audience { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the client id / application id for the registered test app.
    /// </summary>
    public string ClientId { get; set; } = null!;

    /// <summary>
    ///     Gets the applications default scope.
    /// </summary>
    public string DefaultScope =>
        $"{Audience}/.default";

    /// <summary>
    ///     Gets or sets the password of the user.
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the tenant id.
    /// </summary>
    public string TenantId { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the Username of a user registered in AAD. Should be from a test tenant, not a production one. Cannot
    ///     have MFA enabled.
    /// </summary>
    public string Username { get; set; } = null!;
}
