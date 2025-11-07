using Jira.TimeConverters;

using Refit;

namespace Jira;

/// <summary>
/// Interface for Jira Work Item (Issue) REST API operations.
/// Provides methods to search and query work items based on various criteria.
/// </summary>
public interface IJiraIssueApi
{
    /// <summary>
    /// Searches for work items using JQL (Jira Query Language) with enhanced search.
    /// Uses cursor-based pagination with nextPageToken instead of offset-based pagination.
    /// </summary>
    /// <param name="jql">The JQL query string to filter issues.</param>
    /// <param name="nextPageToken">Token for cursor-based pagination to get the next page of results.</param>
    /// <param name="maxResults">The maximum number of items to return (default: 50, max: 100).</param>
    /// <param name="fields">Comma-separated list of fields to include in the response (default: all fields).</param>
    /// <param name="expand">Comma-separated list of parameters to expand (e.g., changelog, renderedFields).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Search results containing matching work items with pagination info.</returns>
    [Get("/rest/api/3/search/jql")]
    Task<JiraSearchResponse> SearchIssuesAsync(
        [AliasAs("jql")] string jql,
        [AliasAs("nextPageToken")] string? nextPageToken = null,
        [AliasAs("maxResults")] int? maxResults = 50,
        [AliasAs("fields")] string? fields = null,
        [AliasAs("expand")] string? expand = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific work item by its key (e.g., PROJ-123).
    /// </summary>
    /// <param name="issueKey">The issue key (e.g., PROJ-123).</param>
    /// <param name="fields">Comma-separated list of fields to include in the response.</param>
    /// <param name="expand">Comma-separated list of parameters to expand.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The work item with the specified key.</returns>
    [Get("/rest/api/3/issue/{issueKey}")]
    Task<JiraIssue> GetIssueByKeyAsync(
        string issueKey,
        [AliasAs("fields")] string? fields = null,
        [AliasAs("expand")] string? expand = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific work item by its ID.
    /// </summary>
    /// <param name="issueId">The issue ID.</param>
    /// <param name="fields">Comma-separated list of fields to include in the response.</param>
    /// <param name="expand">Comma-separated list of parameters to expand.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The work item with the specified ID.</returns>
    [Get("/rest/api/3/issue/{issueId}")]
    Task<JiraIssue> GetIssueByIdAsync(
        string issueId,
        [AliasAs("fields")] string? fields = null,
        [AliasAs("expand")] string? expand = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all worklogs for an issue.
    /// </summary>
    /// <param name="issueKey">The issue key (e.g., PROJ-123).</param>
    /// <param name="startAt">The index of the first item to return (default: 0).</param>
    /// <param name="maxResults">The maximum number of items to return (default: 1048576).</param>
    /// <param name="startedAfter">Only return worklogs started after this date/time.</param>
    /// <param name="startedBefore">Only return worklogs started before this date/time.</param>
    /// <param name="expand">Use expand to include additional information about worklogs in the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of worklogs for the specified issue.</returns>
    [Get("/rest/api/3/issue/{issueKey}/worklog")]
    Task<JiraWorklogsResponse> GetIssueWorklogsAsync(
        string issueKey,
        [AliasAs("startAt")] int? startAt = null,
        [AliasAs("maxResults")] int? maxResults = null,
        [AliasAs("startedAfter")][UnixTimestampMilliseconds] DateTime? startedAfter = null,
        [AliasAs("startedBefore")][UnixTimestampMilliseconds] DateTime? startedBefore = null,
        [AliasAs("expand")] string? expand = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific worklog entry from an issue.
    /// </summary>
    /// <param name="issueKey">The issue key (e.g., PROJ-123).</param>
    /// <param name="worklogId">The ID of the worklog.</param>
    /// <param name="expand">Use expand to include additional information about the worklog in the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The worklog with the specified ID.</returns>
    [Get("/rest/api/3/issue/{issueKey}/worklog/{worklogId}")]
    Task<JiraWorklog> GetWorklogByIdAsync(
        string issueKey,
        string worklogId,
        [AliasAs("expand")] string? expand = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new worklog entry for an issue.
    /// </summary>
    /// <param name="issueKey">The issue key (e.g., PROJ-123).</param>
    /// <param name="request">The worklog creation request data.</param>
    /// <param name="notifyUsers">Whether to notify users about the worklog creation (default: true).</param>
    /// <param name="adjustEstimate">Defines how to update the issue's time tracking fields. Valid values: "new", "leave", "manual", "auto" (default: "auto").</param>
    /// <param name="newEstimate">The new estimate value (required when adjustEstimate is "new"). Automatically converted to Jira format.</param>
    /// <param name="reduceBy">The amount to reduce the estimate by (required when adjustEstimate is "manual"). Automatically converted to Jira format.</param>
    /// <param name="expand">Use expand to include additional information about the worklog in the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created worklog entry.</returns>
    [Post("/rest/api/3/issue/{issueKey}/worklog")]
    Task<JiraWorklog> CreateWorklogAsync(
        string issueKey,
        [Body] CreateJiraWorklogRequest request,
        [AliasAs("notifyUsers")] bool? notifyUsers = null,
        [AliasAs("adjustEstimate")] string? adjustEstimate = null,
        [AliasAs("newEstimate")] TimeSpan? newEstimate = null,
        [AliasAs("reduceBy")] TimeSpan? reduceBy = null,
        [AliasAs("expand")] string? expand = null,
        CancellationToken cancellationToken = default);
}
