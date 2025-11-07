using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents a Jira work item (issue).
/// </summary>
public sealed record JiraIssue
{
    /// <summary>
    /// The unique identifier for the issue.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>
    /// The issue key (e.g., PROJ-123).
    /// </summary>
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    /// <summary>
    /// The URL to view this issue in the Jira web interface.
    /// </summary>
    [JsonPropertyName("self")]
    public required string Self { get; init; }

    /// <summary>
    /// Expansion details for the issue.
    /// </summary>
    [JsonPropertyName("expand")]
    public string? Expand { get; init; }

    /// <summary>
    /// The fields of the issue.
    /// </summary>
    [JsonPropertyName("fields")]
    public JiraIssueFields? Fields { get; init; }

}
