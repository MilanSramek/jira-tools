using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents a Jira project.
/// </summary>
public sealed record JiraProject
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
