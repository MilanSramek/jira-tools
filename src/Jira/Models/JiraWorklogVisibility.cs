using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents visibility settings for a worklog entry.
/// </summary>
public sealed record JiraWorklogVisibility
{
    /// <summary>
    /// Gets or initializes the type of visibility restriction.
    /// Valid values: "group" or "role".
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    /// <summary>
    /// Gets or initializes the value of the visibility restriction.
    /// For type "group": the group name.
    /// For type "role": the role name or ID.
    /// </summary>
    [JsonPropertyName("value")]
    public required string Value { get; init; }

    /// <summary>
    /// Gets or initializes the identifier (name or ID) of the visibility restriction.
    /// Alternative to Value.
    /// </summary>
    [JsonPropertyName("identifier")]
    public string? Identifier { get; init; }
}
