using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents a Jira priority.
/// </summary>
public sealed record JiraPriority
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
