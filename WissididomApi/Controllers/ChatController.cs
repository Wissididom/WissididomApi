using Humanizer;
using Microsoft.AspNetCore.Mvc;
using WissididomApi.Logic;

namespace WissididomApi.Controllers;

[Route("api/[controller]")]
public class ChatController(TwitchApi twitchApi) : ControllerBase
{
    [HttpGet("settings")]
    public async Task<string> ChatSettings(string broadcasterId, string? moderatorId = null)
    {
        try
        {
            var responseArr = await twitchApi.GetChatSettings(broadcasterId, moderatorId);
            if (responseArr is null)
            {
                return "Somehow the chat settings response is null... Maybe there was a network error on the server 🤔";
            }
            if (responseArr.Length < 1)
            {
                return "Somehow the chat settings response was empty... Maybe there was a network error on the server 🤔";
            }
            var response = responseArr[0];
            if (response.Error is not null)
            {
                return $"Twitch Error: {response.Error.Message}";
            }
            List<string> modes = [];
            if (response.EmoteMode)
            {
                modes.Add("emote-only mode enabled");
            }
            if (response.FollowerMode)
            {
                modes.Add($"{TimeSpan.FromMinutes(response.FollowerModeDuration ?? 0).Humanize(4)} follower-only mode enabled");
            }
            // Non-Mod-Chat-Delay if a moderator would have been specified and the app would have had the moderator:read:chat_settings scope
            if (response.SlowMode)
            {
                modes.Add($"{TimeSpan.FromSeconds(response.SlowModeWaitTime ?? 0).Humanize(4)} slow-mode enabled");
            }
            if (response.SubscriberMode)
            {
                modes.Add("subscriber-only mode enabled");
            }
            if (response.UniqueChatMode)
            {
                modes.Add("unique-chat-mode enabled");
            }
            return modes.Count > 0 ? string.Join(" | ", modes) : "All good, no restrictions detected!";
        }
        catch (Exception e)
        {
            return $"Unhandled Exception: {e.Message}";
        }
    }
}
