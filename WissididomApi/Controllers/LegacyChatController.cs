using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using WissididomApi.JsonModels.Api;

namespace WissididomApi.Controllers;

[Route("legacy/chat")]
public class LegacyChatController : ControllerBase
{
    [HttpGet("settings")]
    public IActionResult ChatSettings([FromQuery] LegacyChatSettingsRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var newDomain = Environment.GetEnvironmentVariable("NEW_DOMAIN");
            if (string.IsNullOrWhiteSpace(newDomain))
                return StatusCode(500, "NEW_DOMAIN is not configured.");

            var baseUrl = $"https://{newDomain}/api/chat/settings";
            var redirectUrl = QueryHelpers.AddQueryString(
                baseUrl,
                new Dictionary<string, StringValues>
                {
                    ["BroadcasterLogin"] = request.ChannelName ?? string.Empty
                }
            );

            if (!Uri.TryCreate(redirectUrl, UriKind.Absolute, out var parsedRedirect))
            {
                return BadRequest("Invalid redirect target.");
            }

            return Redirect(parsedRedirect.ToString());
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Unhandled Exception: {e.Message}");
        }
    }
}
