using System.Text.Json.Serialization;

using Jira.TimeConverters;

namespace Jira;

/// <summary>
/// Request model for creating a worklog entry on an issue.
/// </summary>
public sealed record CreateJiraWorklogRequest
{
    /// <summary>
    /// Gets or initializes the comment describing the work done.
    /// </summary>
    [JsonPropertyName("comment")]
    public JiraAdfDocument? Comment { get; init; }

    /// <summary>
    /// Gets or initializes the date and time when the work was started.
    /// Required. Will be serialized in ISO8601 format with +0000 timezone.
    /// </summary>
    [JsonPropertyName("started")]
    [JsonConverter(typeof(DateTimeConverter))]
    public required DateTime Started { get; init; }

    /// <summary>
    /// Gets or initializes the time spent on the work.
    /// </summary>
    [JsonPropertyName("timeSpentSeconds")]
    [JsonConverter(typeof(TimeSpanSecondsConverter))]
    public required TimeSpan TimeSpent { get; init; }

    /// <summary>
    /// Gets or initializes the visibility settings for the worklog.
    /// Defines who can view this worklog entry.
    /// </summary>
    [JsonPropertyName("visibility")]
    public JiraWorklogVisibility? Visibility { get; init; }

    /// <summary>
    /// Gets or initializes additional properties for the worklog.
    /// Used for custom fields or additional metadata.
    /// </summary>
    [JsonPropertyName("properties")]
    public List<JiraWorklogProperty>? Properties { get; init; }
}
