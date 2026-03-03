using System.ComponentModel.DataAnnotations;

namespace WissididomApi.JsonModels.Api;

public class ChatSettingsRequest : IValidatableObject
{
    public string? BroadcasterId { get; init; }
    public string? BroadcasterLogin { get; init; }

    public string? ModeratorId { get; init; }
    public string? ModeratorLogin { get; init; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Broadcaster: exactly one required
        if (string.IsNullOrWhiteSpace(BroadcasterId) == 
            string.IsNullOrWhiteSpace(BroadcasterLogin))
        {
            yield return new ValidationResult(
                "Exactly one of BroadcasterId or BroadcasterLogin must be provided.",
                [nameof(BroadcasterId), nameof(BroadcasterLogin)]);
        }

        // Moderator: optional but mutually exclusive if provided
        if (!string.IsNullOrWhiteSpace(ModeratorId) &&
            !string.IsNullOrWhiteSpace(ModeratorLogin))
        {
            yield return new ValidationResult(
                "ModeratorId and ModeratorLogin are mutually exclusive.",
                [nameof(ModeratorId), nameof(ModeratorLogin)]);
        }
    }
}
