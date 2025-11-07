using Refit;

namespace Clockify;

/// <summary>
/// Interface for Clockify Project Management REST API operations.
/// Provides methods to manage projects, project memberships, and project settings.
/// </summary>
public interface IClockifyProjectApi
{
    /// <summary>
    /// Gets all projects in the specified workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID to get projects from.</param>
    /// <param name="archived">Filter by archived status (optional).</param>
    /// <param name="name">Filter by project name (optional).</param>
    /// <param name="page">Page number for pagination (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 50, max: 200).</param>
    /// <param name="clients">Filter by client IDs (optional).</param>
    /// <param name="contains-client">Include projects that contain client info (optional).</param>
    /// <param name="client-status">Filter by client status (optional).</param>
    /// <param name="users">Filter by user IDs (optional).</param>
    /// <param name="contains-users">Include projects that contain user info (optional).</param>
    /// <param name="user-status">Filter by user status (optional).</param>
    /// <param name="is-template">Filter by template status (optional).</param>
    /// <param name="sort-column">Sort by column (optional).</param>
    /// <param name="sort-order">Sort order: ASC or DESC (optional).</param>
    /// <returns>Array of projects matching the criteria.</returns>
    [Get("/workspaces/{workspaceId}/projects")]
    Task<ClockifyProject[]> GetProjectsAsync(
        string workspaceId,
        [AliasAs("archived")] bool? archived = null,
        [AliasAs("name")] string? name = null,
        [AliasAs("page")] int? page = 1,
        [AliasAs("page-size")] int? pageSize = 50,
        [AliasAs("clients")] string? clients = null,
        [AliasAs("contains-client")] bool? containsClient = null,
        [AliasAs("client-status")] string? clientStatus = null,
        [AliasAs("users")] string? users = null,
        [AliasAs("contains-users")] bool? containsUsers = null,
        [AliasAs("user-status")] string? userStatus = null,
        [AliasAs("is-template")] bool? isTemplate = null,
        [AliasAs("sort-column")] string? sortColumn = null,
        [AliasAs("sort-order")] string? sortOrder = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific project by ID in the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID to retrieve.</param>
    /// <returns>The project's data.</returns>
    [Get("/workspaces/{workspaceId}/projects/{projectId}")]
    Task<ClockifyProject> GetProjectByIdAsync(string workspaceId, string projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new project in the workspace.
    /// Note: This endpoint typically requires project creation permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID to create the project in.</param>
    /// <param name="request">The project creation request data.</param>
    /// <returns>The created project's data.</returns>
    [Post("/workspaces/{workspaceId}/projects")]
    Task<ClockifyProject> CreateProjectAsync(string workspaceId, [Body] CreateProjectRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing project in the workspace.
    /// Note: This endpoint typically requires project management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID to update.</param>
    /// <param name="request">The project update request data.</param>
    /// <returns>The updated project's data.</returns>
    [Put("/workspaces/{workspaceId}/projects/{projectId}")]
    Task<ClockifyProject> UpdateProjectAsync(string workspaceId, string projectId, [Body] UpdateProjectRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a project from the workspace.
    /// Note: This endpoint typically requires project management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID to delete.</param>
    /// <returns>Task representing the delete operation.</returns>
    [Delete("/workspaces/{workspaceId}/projects/{projectId}")]
    Task DeleteProjectAsync(string workspaceId, string projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Archives or unarchives a project.
    /// Note: This endpoint typically requires project management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID to archive/unarchive.</param>
    /// <param name="archive">Whether to archive (true) or unarchive (false) the project.</param>
    /// <returns>The updated project's data.</returns>
    [Patch("/workspaces/{workspaceId}/projects/{projectId}")]
    Task<ClockifyProject> ArchiveProjectAsync(string workspaceId, string projectId, [Body] object archive, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets project memberships (users assigned to the project).
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="page">Page number for pagination (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 50, max: 200).</param>
    /// <returns>Array of project memberships.</returns>
    [Get("/workspaces/{workspaceId}/projects/{projectId}/memberships")]
    Task<ClockifyProjectMembership[]> GetProjectMembershipsAsync(
        string workspaceId,
        string projectId,
        [AliasAs("page")] int? page = 1,
        [AliasAs("page-size")] int? pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds users to a project.
    /// Note: This endpoint typically requires project management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="memberships">Array of memberships to add.</param>
    /// <returns>Array of added memberships.</returns>
    [Post("/workspaces/{workspaceId}/projects/{projectId}/memberships")]
    Task<ClockifyProjectMembership[]> AddProjectMembershipsAsync(
        string workspaceId,
        string projectId,
        [Body] ClockifyProjectMembership[] memberships,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a user's membership in a project.
    /// Note: This endpoint typically requires project management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="userId">The user ID whose membership to update.</param>
    /// <param name="membership">The membership update data.</param>
    /// <returns>The updated membership data.</returns>
    [Put("/workspaces/{workspaceId}/projects/{projectId}/memberships/{userId}")]
    Task<ClockifyProjectMembership> UpdateProjectMembershipAsync(
        string workspaceId,
        string projectId,
        string userId,
        [Body] ClockifyProjectMembership membership,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a user from a project.
    /// Note: This endpoint typically requires project management permissions.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <param name="userId">The user ID to remove from the project.</param>
    /// <returns>Task representing the remove operation.</returns>
    [Delete("/workspaces/{workspaceId}/projects/{projectId}/memberships/{userId}")]
    Task RemoveProjectMembershipAsync(string workspaceId, string projectId, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets project template information.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="projectId">The project ID.</param>
    /// <returns>The project template data.</returns>
    [Get("/workspaces/{workspaceId}/projects/{projectId}/template")]
    Task<ClockifyProject> GetProjectTemplateAsync(string workspaceId, string projectId, CancellationToken cancellationToken = default);
}