using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents a Jira status.
/// </summary>
public sealed record JiraStatus
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("statusCategory")]
    public JiraStatusCategory? StatusCategory { get; init; }
}
