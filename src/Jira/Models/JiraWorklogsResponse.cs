using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents the response from the get worklogs endpoint.
/// </summary>
public sealed record JiraWorklogsResponse
{
    /// <summary>
    /// The index of the first item returned in the page of results (page offset).
    /// </summary>
    [JsonPropertyName("startAt")]
    public int StartAt { get; init; }

    /// <summary>
    /// The maximum number of results that can be returned in the page.
    /// </summary>
    [JsonPropertyName("maxResults")]
    public int MaxResults { get; init; }

    /// <summary>
    /// The total number of worklogs available.
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; init; }

    /// <summary>
    /// The list of worklogs.
    /// </summary>
    [JsonPropertyName("worklogs")]
    public required List<JiraWorklog> Worklogs { get; init; }
}
