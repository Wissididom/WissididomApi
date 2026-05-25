using System.ComponentModel.DataAnnotations;

namespace WissididomApi.JsonModels.Api;

public class LegacyChatSettingsRequest : IValidatableObject
{
    public string? ChannelName { get; init; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Broadcaster: exactly one required
        if (string.IsNullOrWhiteSpace(ChannelName))
        {
            yield return new ValidationResult(
                "Please specify a channelName!",
                [nameof(ChannelName)]);
        }
    }
}
