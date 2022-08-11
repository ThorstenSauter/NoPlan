using System.Text.Json.Serialization;

namespace NoPlan.Api.Tests.Integration.Authentication;

public sealed class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;
}
