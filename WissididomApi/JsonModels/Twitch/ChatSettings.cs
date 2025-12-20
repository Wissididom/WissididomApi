using System.Text.Json.Serialization;

namespace WissididomApi.JsonModels.Twitch;

public class ChatSettings
{
    [JsonPropertyName("broadcaster_id")]
    public string? BroadcasterId { get; set; }
    [JsonPropertyName("emote_mode")]
    public bool EmoteMode { get; set; }
    [JsonPropertyName("follower_mode")]
    public bool FollowerMode { get; set; }
    [JsonPropertyName("follower_mode_duration")]
    public int? FollowerModeDuration { get; set; }
    [JsonPropertyName("moderator_id")]
    public string? ModeratorId { get; set; }
    [JsonPropertyName("non_moderator_chat_delay")]
    public bool NonModeratorChatDelay { get; set; }
    [JsonPropertyName("non_moderator_chat_delay_duration")]
    public int? NonModeratorChatDelayDuration { get; set; }
    [JsonPropertyName("slow_mode")]
    public bool SlowMode { get; set; }
    [JsonPropertyName("slow_mode_wait_time")]
    public int? SlowModeWaitTime { get; set; }
    [JsonPropertyName("subscriber_mode")]
    public bool SubscriberMode { get; set; }
    [JsonPropertyName("unique_chat_mode")]
    public bool UniqueChatMode { get; set; }
    [JsonIgnore]
    public TwitchError? Error { get; set; }
}
