using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents a Jira resolution.
/// </summary>
public sealed record JiraResolution
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }
}
