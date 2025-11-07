using Refit;

namespace Clockify;

/// <summary>
/// Interface for Clockify Task Management REST API operations.
/// Provides methods to manage tasks within projects.
/// </summary>
public interface IClockifyTaskApi
{
    /// <summary>
    /// Gets all tasks for a specific project in the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID to get tasks from.</param>
    /// <param name="is-active">Filter by active status (optional).</param>
    /// <param name="name">Filter by task name (optional).</param>
    /// <param name="page">Page number for pagination (default: 1).</param>
    /// <param name="page-size">Number of items per page (default: 50, max: 200).</param>
    /// <param name="sort-column">Sort by column (optional).</param>
    /// <param name="sort-order">Sort order: ASC or DESC (optional).</param>
    /// <returns>Array of tasks matching the criteria.</returns>
    [Get("/workspaces/{workspaceId}/projects/{projectId}/tasks")]
    Task<ClockifyTask[]> GetTasksAsync(
        string workspaceId,
        string projectId,
        [AliasAs("is-active")] bool? isActive = null,
        [AliasAs("name")] string? name = null,
        [AliasAs("page")] int? page = 1,
        [AliasAs("page-size")] int? pageSize = 50,
        [AliasAs("sort-column")] string? sortColumn = null,
        [AliasAs("sort-order")] string? sortOrder = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific task by ID in the project.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="taskId">The task ID to retrieve.</param>
    /// <returns>The task's data.</returns>
    [Get("/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}")]
    Task<ClockifyTask> GetTaskByIdAsync(string workspaceId, string projectId, string taskId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new task in the specified project.
    /// Note: This endpoint typically requires task creation permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID to create the task in.</param>
    /// <param name="request">The task creation request data.</param>
    /// <returns>The created task's data.</returns>
    [Post("/workspaces/{workspaceId}/projects/{projectId}/tasks")]
    Task<ClockifyTask> CreateTaskAsync(string workspaceId, string projectId, [Body] CreateTaskRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing task in the project.
    /// Note: This endpoint typically requires task management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="taskId">The task ID to update.</param>
    /// <param name="request">The task update request data.</param>
    /// <returns>The updated task's data.</returns>
    [Put("/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}")]
    Task<ClockifyTask> UpdateTaskAsync(string workspaceId, string projectId, string taskId, [Body] UpdateTaskRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a task from the project.
    /// Note: This endpoint typically requires task management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="taskId">The task ID to delete.</param>
    /// <returns>Task representing the delete operation.</returns>
    [Delete("/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}")]
    Task DeleteTaskAsync(string workspaceId, string projectId, string taskId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the status of a task (ACTIVE or DONE).
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="taskId">The task ID to update status for.</param>
    /// <param name="status">The new status object.</param>
    /// <returns>The updated task's data.</returns>
    [Patch("/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}")]
    Task<ClockifyTask> SetTaskStatusAsync(string workspaceId, string projectId, string taskId, [Body] object status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Assigns users to a task.
    /// Note: This endpoint typically requires task management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="taskId">The task ID to assign users to.</param>
    /// <param name="assigneeIds">Array of user IDs to assign to the task.</param>
    /// <returns>The updated task's data.</returns>
    [Put("/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/assignees")]
    Task<ClockifyTask> AssignUsersToTaskAsync(
        string workspaceId,
        string projectId,
        string taskId,
        [Body] string[] assigneeIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes users from a task assignment.
    /// Note: This endpoint typically requires task management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="taskId">The task ID to remove users from.</param>
    /// <param name="assigneeIds">Array of user IDs to remove from the task.</param>
    /// <returns>The updated task's data.</returns>
    [Delete("/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/assignees")]
    Task<ClockifyTask> RemoveUsersFromTaskAsync(
        string workspaceId,
        string projectId,
        string taskId,
        [Body] string[] assigneeIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets hourly rate for a task.
    /// Note: This endpoint typically requires task management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="taskId">The task ID to set rate for.</param>
    /// <param name="hourlyRate">The hourly rate data.</param>
    /// <returns>The updated task's data.</returns>
    [Post("/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/hourlyRate")]
    Task<ClockifyTask> SetTaskHourlyRateAsync(
        string workspaceId,
        string projectId,
        string taskId,
        [Body] ClockifyHourlyRate hourlyRate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets cost rate for a task.
    /// Note: This endpoint typically requires task management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="taskId">The task ID to set cost rate for.</param>
    /// <param name="costRate">The cost rate data.</param>
    /// <returns>The updated task's data.</returns>
    [Post("/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/costRate")]
    Task<ClockifyTask> SetTaskCostRateAsync(
        string workspaceId,
        string projectId,
        string taskId,
        [Body] ClockifyHourlyRate costRate,
        CancellationToken cancellationToken = default);
}