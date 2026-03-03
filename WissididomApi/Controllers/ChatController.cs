using Humanizer;
using Microsoft.AspNetCore.Mvc;
using WissididomApi.JsonModels.Api;
using WissididomApi.Logic;

namespace WissididomApi.Controllers;

[Route("api/[controller]")]
public class ChatController(TwitchApi twitchApi) : ControllerBase
{
    [HttpGet("settings")]
    public async Task<IActionResult> ChatSettings([FromQuery] ChatSettingsRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var broadcasterId = request.BroadcasterId;
            if (broadcasterId is null)
            {
                var broadcasterData = await twitchApi.GetUsers(null, [request.BroadcasterLogin!]);
                if (broadcasterData is not null && broadcasterData.Length > 0)
                    broadcasterId = broadcasterData[0].Id;
            }
            if (broadcasterId is null) return BadRequest("Failed to resolve broadcaster id. Did you make a typo?");
            var moderatorId = request.ModeratorId;
            if (moderatorId is null && request.ModeratorLogin is not null)
            {
                var moderatorData = await twitchApi.GetUsers(null, [request.ModeratorLogin]);
                if (moderatorData is not null && moderatorData.Length > 0)
                    moderatorId = moderatorData[0].Id;
            }
            var responseArr = await twitchApi.GetChatSettings(broadcasterId, moderatorId);
            if (responseArr is null)
            {
                return StatusCode(500, "Somehow the chat settings response is null... Maybe there was a network error on the server 🤔");
            }
            if (responseArr.Length < 1)
            {
                return StatusCode(500, "Somehow the chat settings response from Twitch was empty...");
            }
            var response = responseArr[0];
            if (response.Error is not null)
            {
                return StatusCode(response.Error.Status, $"Twitch Error: {response.Error.Message}");
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
            return Ok(modes.Count > 0 ? string.Join(" | ", modes) : "All good, no restrictions detected!");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Unhandled Exception: {e.Message}");
        }
    }
}
