using System.Text.Json.Serialization;

namespace WissididomApi.JsonModels.Twitch;

public class GetChatSettingsResponse
{
    [JsonPropertyName("data")]
    public ChatSettings[]? Data { get; set; }
}
