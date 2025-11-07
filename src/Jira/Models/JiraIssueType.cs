using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents a Jira issue type.
/// </summary>
public sealed record JiraIssueType
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("iconUrl")]
    public string? IconUrl { get; init; }
}
