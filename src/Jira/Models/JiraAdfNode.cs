using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents a node in an ADF document.
/// </summary>
public sealed record JiraAdfNode
{
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("content")]
    public List<JiraAdfNode>? Content { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }
}
