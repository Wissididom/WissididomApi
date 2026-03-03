using System.Text.Json.Serialization;

namespace WissididomApi.JsonModels.Twitch;

public class GetUsersResponse
{
    [JsonPropertyName("data")]
    public TwitchUser[]? Data { get; set; }
}
