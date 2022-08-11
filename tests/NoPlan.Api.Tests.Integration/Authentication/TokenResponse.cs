using System.Text.Json.Serialization;

namespace NoPlan.Api.Tests.Integration.Authentication;

/// <summary>
///     The token response after performing the OAuth 2.0 ROPC flow.
/// </summary>
public sealed class TokenResponse
{
    /// <summary>
    ///     The access token used to access the defined application.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;
}
