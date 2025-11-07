using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents a collection of work log entries in a Jira issue.
/// </summary>
public sealed record JiraWorklogCollection
{
    /// <summary>
    /// The list of work log entries.
    /// </summary>
    [JsonPropertyName("worklogs")]
    public List<JiraWorklog>? Worklogs { get; init; }

    /// <summary>
    /// The maximum number of results returned.
    /// </summary>
    [JsonPropertyName("maxResults")]
    public int? MaxResults { get; init; }

    /// <summary>
    /// The total number of work logs available.
    /// </summary>
    [JsonPropertyName("total")]
    public int? Total { get; init; }

    /// <summary>
    /// The index of the first result returned.
    /// </summary>
    [JsonPropertyName("startAt")]
    public int? StartAt { get; init; }
}
