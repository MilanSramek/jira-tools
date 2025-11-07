using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents the avatar URLs for a Jira user in different sizes.
/// </summary>
public sealed record JiraUserAvatarUrls
{
    /// <summary>
    /// URL for the 16x16 pixel avatar image.
    /// </summary>
    [JsonPropertyName("16x16")]
    public string? Size16x16 { get; init; }

    /// <summary>
    /// URL for the 24x24 pixel avatar image.
    /// </summary>
    [JsonPropertyName("24x24")]
    public string? Size24x24 { get; init; }

    /// <summary>
    /// URL for the 32x32 pixel avatar image.
    /// </summary>
    [JsonPropertyName("32x32")]
    public string? Size32x32 { get; init; }

    /// <summary>
    /// URL for the 48x48 pixel avatar image.
    /// </summary>
    [JsonPropertyName("48x48")]
    public string? Size48x48 { get; init; }
}
