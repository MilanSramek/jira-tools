using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents a Jira status category.
/// </summary>
public sealed record JiraStatusCategory
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
