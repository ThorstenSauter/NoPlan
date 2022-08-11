namespace NoPlan.Api.Tests.Integration.Authentication;

public sealed class UserAuthenticationSettings
{
    /// <summary>
    ///     The URL to acquire the access token from, e.g.
    ///     https://login.microsoftonline.com/your-aad-tenant-id/oauth2/v2.0/token
    /// </summary>
    public string TokenUrl => $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/token";

    /// <summary>
    ///     The tenant id.
    /// </summary>
    public string TenantId { get; set; } = null!;

    /// <summary>
    ///     Client id / application id for the registered test app.
    /// </summary>
    public string ClientId { get; set; } = null!;

    /// <summary>
    ///     Secret for the registered test app.
    /// </summary>
    public string ClientSecret { get; set; } = null!;

    /// <summary>
    ///     Username of a user registered in AAD. Should be from a test tenant, not a production one. Cannot have MFA enabled.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    ///     Password of the user.
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    ///     The application audience.
    /// </summary>
    public string Audience { get; set; } = null!;
}
