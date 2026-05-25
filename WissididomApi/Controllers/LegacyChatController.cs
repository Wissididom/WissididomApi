using Microsoft.AspNetCore.Mvc;
using WissididomApi.JsonModels.Api;
using WissididomApi.Logic;

namespace WissididomApi.Controllers;

[Route("legacy/chat")]
public class LegacyChatController(TwitchApi twitchApi) : ControllerBase
{
    [HttpGet("settings")]
    public IActionResult ChatSettings([FromQuery] LegacyChatSettingsRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Redirect(
                $"https://{Environment.GetEnvironmentVariable("NEW_DOMAIN")}/api/chat/settings?BroadcasterLogin={Uri.EscapeDataString(request.ChannelName ?? "")}"
            );
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Unhandled Exception: {e.Message}");
        }
    }
}
