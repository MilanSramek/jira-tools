using System.Text.Json;
using System.Text.Json.Serialization;

using Jira.TimeConverters;

namespace Jira;

/// <summary>
/// Represents a work log entry in a Jira issue.
/// </summary>
public sealed record JiraWorklog
{
    /// <summary>
    /// The unique identifier of the work log entry.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// The URL to this work log entry.
    /// </summary>
    [JsonPropertyName("self")]
    public string? Self { get; init; }

    /// <summary>
    /// The author of the work log entry.
    /// </summary>
    [JsonPropertyName("author")]
    public JiraUser? Author { get; init; }

    /// <summary>
    /// The user who last updated the work log entry.
    /// </summary>
    [JsonPropertyName("updateAuthor")]
    public JiraUser? UpdateAuthor { get; init; }

    /// <summary>
    /// Comment describing the work performed. Can be a string or Atlassian Document Format (ADF) object.
    /// </summary>
    [JsonPropertyName("comment")]
    public JiraAdfDocument? Comment { get; init; }

    /// <summary>
    /// When the work was started.
    /// </summary>
    [JsonPropertyName("started")]
    public DateTime? Started { get; init; }

    /// <summary>
    /// Time spent in human-readable format (e.g., "1h", "2h 30m").
    /// </summary>
    [JsonPropertyName("timeSpent")]
    public string? TimeSpentJiraFormat { get; init; }

    /// <summary>
    /// Time spent.
    /// </summary>
    [JsonPropertyName("timeSpentSeconds")]
    [JsonConverter(typeof(TimeSpanSecondsConverter))]
    public TimeSpan TimeSpent { get; init; }

    /// <summary>
    /// The ID of the issue this work log belongs to.
    /// </summary>
    [JsonPropertyName("issueId")]
    public string? IssueId { get; init; }

    /// <summary>
    /// When the work log was created.
    /// </summary>
    [JsonPropertyName("created")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// When the work log was last updated.
    /// </summary>
    [JsonPropertyName("updated")]
    public DateTime? UpdatedAt { get; init; }
}
