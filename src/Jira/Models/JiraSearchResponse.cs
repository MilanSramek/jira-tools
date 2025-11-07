using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents the response from a Jira enhanced search query.
/// Uses cursor-based pagination with nextPageToken.
/// </summary>
public sealed record JiraSearchResponse
{
    /// <summary>
    /// The search results containing matching issues.
    /// </summary>
    [JsonPropertyName("issues")]
    public required List<JiraIssue> Issues { get; init; }

    /// <summary>
    /// Indicates whether this is the last page of results.
    /// </summary>
    [JsonPropertyName("isLast")]
    public bool IsLast { get; init; }

    /// <summary>
    /// Token for retrieving the next page of results. Only present when isLast is false.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    public string? NextPageToken { get; init; }
}
