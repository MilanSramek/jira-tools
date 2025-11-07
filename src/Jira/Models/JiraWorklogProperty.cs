using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents a property for a worklog entry.
/// </summary>
public sealed record JiraWorklogProperty
{
    /// <summary>
    /// Gets or initializes the key of the property.
    /// </summary>
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    /// <summary>
    /// Gets or initializes the value of the property.
    /// </summary>
    [JsonPropertyName("value")]
    public required object Value { get; init; }
}
