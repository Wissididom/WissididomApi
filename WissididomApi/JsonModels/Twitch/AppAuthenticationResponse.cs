using System.Text.Json.Serialization;

namespace WissididomApi.JsonModels.Twitch;

public class AppAuthenticationResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    private int? _expiresIn;

    [JsonPropertyName("expires_in")]
    public int? ExpiresIn
    {
        get => _expiresIn;
        set
        {
            _expiresIn = value;
            ExpiresAt = value.HasValue ? DateTime.UtcNow.AddSeconds(value.Value) : null;
        }
    }
    [JsonIgnore]
    public DateTime? ExpiresAt { get; private set; }
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }
    [JsonIgnore]
    public TwitchError? Error { get; set; }
}
