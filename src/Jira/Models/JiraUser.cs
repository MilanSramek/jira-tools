using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents a Jira user.
/// </summary>
public sealed record JiraUser
{
    /// <summary>
    /// The URL to this user resource.
    /// </summary>
    [JsonPropertyName("self")]
    public string? Self { get; init; }

    /// <summary>
    /// The unique account identifier for the user.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required string AccountId { get; init; }

    /// <summary>
    /// The email address of the user.
    /// </summary>
    [JsonPropertyName("emailAddress")]
    public string? EmailAddress { get; init; }

    /// <summary>
    /// The URLs for the user's avatar images in different sizes.
    /// </summary>
    [JsonPropertyName("avatarUrls")]
    public JiraUserAvatarUrls? AvatarUrls { get; init; }

    /// <summary>
    /// The display name of the user.
    /// </summary>
    [JsonPropertyName("displayName")]
    public required string DisplayName { get; init; }

    /// <summary>
    /// Whether the user is active in the system.
    /// </summary>
    [JsonPropertyName("active")]
    public bool Active { get; init; }

    /// <summary>
    /// The time zone of the user.
    /// </summary>
    [JsonPropertyName("timeZone")]
    public string? TimeZone { get; init; }

    /// <summary>
    /// The account type of the user (e.g., "atlassian", "app", "customer").
    /// </summary>
    [JsonPropertyName("accountType")]
    public string? AccountType { get; init; }
}
