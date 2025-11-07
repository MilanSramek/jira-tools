using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents the fields of a Jira work item.
/// </summary>
public sealed record JiraIssueFields
{
    /// <summary>
    /// The summary (title) of the issue.
    /// </summary>
    [JsonPropertyName("summary")]
    public string? Summary { get; init; }

    /// <summary>
    /// The description of the issue. Can be a string or Atlassian Document Format (ADF) object.
    /// </summary>
    [JsonPropertyName("description")]
    public JsonElement? Description { get; init; }

    /// <summary>
    /// The status of the issue.
    /// </summary>
    [JsonPropertyName("status")]
    public JiraStatus? Status { get; init; }

    /// <summary>
    /// The issue type (e.g., Story, Task, Bug).
    /// </summary>
    [JsonPropertyName("issuetype")]
    public JiraIssueType? IssueType { get; init; }

    /// <summary>
    /// The project this issue belongs to.
    /// </summary>
    [JsonPropertyName("project")]
    public JiraProject? Project { get; init; }

    /// <summary>
    /// The assignee of the issue.
    /// </summary>
    [JsonPropertyName("assignee")]
    public JiraUser? Assignee { get; init; }

    /// <summary>
    /// The reporter of the issue.
    /// </summary>
    [JsonPropertyName("reporter")]
    public JiraUser? Reporter { get; init; }

    /// <summary>
    /// The priority of the issue.
    /// </summary>
    [JsonPropertyName("priority")]
    public JiraPriority? Priority { get; init; }

    /// <summary>
    /// When the issue was created.
    /// </summary>
    [JsonPropertyName("created")]
    public DateTime? Created { get; init; }

    /// <summary>
    /// When the issue was last updated.
    /// </summary>
    [JsonPropertyName("updated")]
    public DateTime? Updated { get; init; }

    /// <summary>
    /// The resolution of the issue.
    /// </summary>
    [JsonPropertyName("resolution")]
    public JiraResolution? Resolution { get; init; }

    /// <summary>
    /// Labels associated with the issue.
    /// </summary>
    [JsonPropertyName("labels")]
    public List<string>? Labels { get; init; }

    /// <summary>
    /// Work log entries for this issue.
    /// </summary>
    [JsonPropertyName("worklog")]
    public JiraWorklogCollection? Worklog { get; init; }
}
