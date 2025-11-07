using Refit;

namespace Clockify;

/// <summary>
/// Interface for Clockify Time Entry Management REST API operations.
/// Provides methods to manage time entries, track time, and generate reports.
/// </summary>
public interface IClockifyTimeEntryApi
{
    /// <summary>
    /// Gets time entries for the current user in the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="description">Filter by description (optional).</param>
    /// <param name="start">Filter by start date (ISO 8601) (optional).</param>
    /// <param name="end">Filter by end date (ISO 8601) (optional).</param>
    /// <param name="project">Filter by project ID (optional).</param>
    /// <param name="task">Filter by task ID (optional).</param>
    /// <param name="tags">Filter by tag IDs (optional).</param>
    /// <param name="project-required">Require project to be set (optional).</param>
    /// <param name="task-required">Require task to be set (optional).</param>
    /// <param name="consider-duration-format">Consider duration format (optional).</param>
    /// <param name="hydrated">Include related data (optional).</param>
    /// <param name="in-progress">Filter by in-progress entries (optional).</param>
    /// <param name="page">Page number for pagination (default: 1).</param>
    /// <param name="page-size">Number of items per page (default: 50, max: 200).</param>
    /// <returns>Array of time entries matching the criteria.</returns>
    [Get("/workspaces/{workspaceId}/user/{userId}/time-entries")]
    Task<ClockifyTimeEntry[]> GetTimeEntriesAsync(
        string workspaceId,
        string userId,
        [AliasAs("description")] string? description = null,
        [AliasAs("start")] DateTime? start = null,
        [AliasAs("end")] DateTime? end = null,
        [AliasAs("project")] string? project = null,
        [AliasAs("task")] string? task = null,
        [AliasAs("tags")] string? tags = null,
        [AliasAs("project-required")] bool? projectRequired = null,
        [AliasAs("task-required")] bool? taskRequired = null,
        [AliasAs("consider-duration-format")] bool? considerDurationFormat = null,
        [AliasAs("hydrated")] bool? hydrated = null,
        [AliasAs("in-progress")] bool? inProgress = null,
        [AliasAs("page")] int? page = 1,
        [AliasAs("page-size")] int? pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all time entries in the workspace (admin only).
    /// Note: This endpoint typically requires workspace administrator permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="description">Filter by description (optional).</param>
    /// <param name="start">Filter by start date (ISO 8601) (optional).</param>
    /// <param name="end">Filter by end date (ISO 8601) (optional).</param>
    /// <param name="project">Filter by project ID (optional).</param>
    /// <param name="task">Filter by task ID (optional).</param>
    /// <param name="user">Filter by user ID (optional).</param>
    /// <param name="tags">Filter by tag IDs (optional).</param>
    /// <param name="billable">Filter by billable status (optional).</param>
    /// <param name="hydrated">Include related data (optional).</param>
    /// <param name="page">Page number for pagination (default: 1).</param>
    /// <param name="page-size">Number of items per page (default: 50, max: 200).</param>
    /// <returns>Array of all time entries matching the criteria.</returns>
    [Get("/workspaces/{workspaceId}/time-entries")]
    Task<ClockifyTimeEntry[]> GetAllTimeEntriesAsync(
        string workspaceId,
        [AliasAs("description")] string? description = null,
        [AliasAs("start")] DateTime? start = null,
        [AliasAs("end")] DateTime? end = null,
        [AliasAs("project")] string? project = null,
        [AliasAs("task")] string? task = null,
        [AliasAs("user")] string? user = null,
        [AliasAs("tags")] string? tags = null,
        [AliasAs("billable")] bool? billable = null,
        [AliasAs("hydrated")] bool? hydrated = null,
        [AliasAs("page")] int? page = 1,
        [AliasAs("page-size")] int? pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific time entry by ID.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="timeEntryId">The time entry ID to retrieve.</param>
    /// <param name="hydrated">Include related data (optional).</param>
    /// <returns>The time entry's data.</returns>
    [Get("/workspaces/{workspaceId}/time-entries/{timeEntryId}")]
    Task<ClockifyTimeEntry> GetTimeEntryByIdAsync(
        string workspaceId,
        string timeEntryId,
        [AliasAs("hydrated")] bool? hydrated = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new time entry for the current user.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="request">The time entry creation request data.</param>
    /// <returns>The created time entry's data.</returns>
    [Post("/workspaces/{workspaceId}/time-entries")]
    Task<ClockifyTimeEntry> CreateTimeEntryAsync(string workspaceId, [Body] CreateTimeEntryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing time entry.
    /// Note: Users can typically only update their own time entries.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="timeEntryId">The time entry ID to update.</param>
    /// <param name="request">The time entry update request data.</param>
    /// <returns>The updated time entry's data.</returns>
    [Put("/workspaces/{workspaceId}/time-entries/{timeEntryId}")]
    Task<ClockifyTimeEntry> UpdateTimeEntryAsync(string workspaceId, string timeEntryId, [Body] UpdateTimeEntryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a time entry.
    /// Note: Users can typically only delete their own time entries.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="timeEntryId">The time entry ID to delete.</param>
    /// <returns>Task representing the delete operation.</returns>
    [Delete("/workspaces/{workspaceId}/time-entries/{timeEntryId}")]
    Task DeleteTimeEntryAsync(string workspaceId, string timeEntryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a new time entry (timer functionality).
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="request">The time entry start request data.</param>
    /// <returns>The started time entry's data.</returns>
    [Post("/workspaces/{workspaceId}/time-entries")]
    Task<ClockifyTimeEntry> StartTimeEntryAsync(string workspaceId, [Body] CreateTimeEntryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops the currently running time entry for the user.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID whose timer to stop.</param>
    /// <param name="end">End time data.</param>
    /// <returns>The stopped time entry's data.</returns>
    [Patch("/workspaces/{workspaceId}/user/{userId}/time-entries")]
    Task<ClockifyTimeEntry> StopTimeEntryAsync(string workspaceId, string userId, [Body] object end, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current running time entry for a user.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID to get current timer for.</param>
    /// <returns>The currently running time entry, or null if no timer is running.</returns>
    [Get("/workspaces/{workspaceId}/user/{userId}/time-entries/in-progress")]
    Task<ClockifyTimeEntry?> GetRunningTimeEntryAsync(string workspaceId, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates multiple time entries at once (bulk operation).
    /// Note: This endpoint typically requires appropriate permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="timeEntryIds">Array of time entry IDs to update.</param>
    /// <param name="updates">The bulk update request data.</param>
    /// <returns>Array of updated time entries.</returns>
    [Patch("/workspaces/{workspaceId}/time-entries/bulk")]
    Task<ClockifyTimeEntry[]> BulkUpdateTimeEntriesAsync(
        string workspaceId,
        [AliasAs("timeEntryIds")] string[] timeEntryIds,
        [Body] UpdateTimeEntryRequest updates,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes multiple time entries at once (bulk operation).
    /// Note: This endpoint typically requires appropriate permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="timeEntryIds">Array of time entry IDs to delete.</param>
    /// <returns>Task representing the bulk delete operation.</returns>
    [Delete("/workspaces/{workspaceId}/time-entries/bulk")]
    Task BulkDeleteTimeEntriesAsync(string workspaceId, [Body] string[] timeEntryIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Duplicates a time entry.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="timeEntryId">The time entry ID to duplicate.</param>
    /// <returns>The duplicated time entry's data.</returns>
    [Post("/workspaces/{workspaceId}/time-entries/{timeEntryId}/duplicate")]
    Task<ClockifyTimeEntry> DuplicateTimeEntryAsync(string workspaceId, string timeEntryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Converts a time entry to a different format or moves it to a different project/task.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="timeEntryId">The time entry ID to convert.</param>
    /// <param name="conversion">The conversion request data.</param>
    /// <returns>The converted time entry's data.</returns>
    [Put("/workspaces/{workspaceId}/time-entries/{timeEntryId}/convert")]
    Task<ClockifyTimeEntry> ConvertTimeEntryAsync(string workspaceId, string timeEntryId, [Body] object conversion, CancellationToken cancellationToken = default);
}