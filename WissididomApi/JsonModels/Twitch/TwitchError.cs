using System.Text.Json.Serialization;

namespace WissididomApi.JsonModels.Twitch;

public class TwitchError
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }
    [JsonPropertyName("status")]
    public int Status { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
