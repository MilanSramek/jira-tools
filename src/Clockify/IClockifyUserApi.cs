using Refit;

namespace Clockify;

/// <summary>
/// Interface for Clockify User Management REST API operations.
/// Provides methods to manage users, memberships, settings, and invitations.
/// </summary>
public interface IClockifyUserApi
{
    /// <summary>
    /// Gets the current authenticated user's profile information.
    /// </summary>
    /// <returns>The current user's profile data.</returns>
    [Get("/user")]
    Task<ClockifyUser> GetCurrentUserAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all users in the specified workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID to get users from.</param>
    /// <param name="email">Filter by user email (optional).</param>
    /// <param name="name">Filter by user name (optional).</param>
    /// <param name="page">Page number for pagination (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 50, max: 200).</param>
    /// <param name="status">Filter by user status: ACTIVE, PENDING, DECLINED, INACTIVE (optional).</param>
    /// <param name="membershipsTargetId">Filter by membership target ID (optional).</param>
    /// <param name="membershipsStatus">Filter by membership status (optional).</param>
    /// <param name="includeRoles">Include user roles in response (optional).</param>
    /// <returns>Array of users matching the criteria.</returns>
    [Get("/workspaces/{workspaceId}/users")]
    Task<ClockifyUser[]> GetUsersAsync(
        string workspaceId,
        [AliasAs("email")] string? email = null,
        [AliasAs("name")] string? name = null,
        [AliasAs("page")] int? page = 1,
        [AliasAs("page-size")] int? pageSize = 50,
        [AliasAs("status")] string? status = null,
        [AliasAs("memberships-target-id")] string? membershipsTargetId = null,
        [AliasAs("memberships-status")] string? membershipsStatus = null,
        [AliasAs("includeRoles")] bool? includeRoles = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific user by ID in the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID to retrieve.</param>
    /// <returns>The user's profile data.</returns>
    [Get("/workspaces/{workspaceId}/users/{userId}")]
    Task<ClockifyUser> GetUserByIdAsync(string workspaceId, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user in the workspace.
    /// Note: This endpoint is typically available only to workspace administrators.
    /// </summary>
    /// <param name="workspaceId">The workspace ID to create the user in.</param>
    /// <param name="request">The user creation request data.</param>
    /// <returns>The created user's data.</returns>
    [Post("/workspaces/{workspaceId}/users")]
    Task<ClockifyUser> CreateUserAsync(string workspaceId, [Body] CreateUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user in the workspace.
    /// Note: This endpoint is typically available only to workspace administrators or the user themselves.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID to update.</param>
    /// <param name="request">The user update request data.</param>
    /// <returns>The updated user's data.</returns>
    [Put("/workspaces/{workspaceId}/users/{userId}")]
    Task<ClockifyUser> UpdateUserAsync(string workspaceId, string userId, [Body] UpdateUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a user from the workspace (soft delete).
    /// Note: This endpoint is typically available only to workspace administrators.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID to remove.</param>
    /// <returns>Task representing the delete operation.</returns>
    [Delete("/workspaces/{workspaceId}/users/{userId}")]
    Task DeleteUserAsync(string workspaceId, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invites users to join the workspace by email.
    /// Note: This endpoint is typically available only to workspace administrators.
    /// </summary>
    /// <param name="workspaceId">The workspace ID to invite users to.</param>
    /// <param name="request">The invitation request containing emails and optional memberships.</param>
    /// <returns>Array of invited users.</returns>
    [Post("/workspaces/{workspaceId}/users/invite")]
    Task<ClockifyUser[]> InviteUsersAsync(string workspaceId, [Body] InviteUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current user's settings and preferences.
    /// </summary>
    /// <returns>The current user's settings.</returns>
    [Get("/user/settings")]
    Task<ClockifyUserSettings> GetCurrentUserSettingsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the current user's settings and preferences.
    /// </summary>
    /// <param name="request">The settings update request.</param>
    /// <returns>The updated user settings.</returns>
    [Put("/user/settings")]
    Task<ClockifyUserSettings> UpdateCurrentUserSettingsAsync([Body] UpdateUserSettingsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets user memberships across all workspaces for the current user.
    /// </summary>
    /// <returns>Array of user memberships.</returns>
    [Get("/user/memberships")]
    Task<ClockifyMembership[]> GetCurrentUserMembershipsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the user's membership in a specific workspace.
    /// Note: Some membership properties may require administrator privileges.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID whose membership to update.</param>
    /// <param name="membership">The membership update data.</param>
    /// <returns>The updated membership data.</returns>
    [Put("/workspaces/{workspaceId}/users/{userId}/membership")]
    Task<ClockifyMembership> UpdateUserMembershipAsync(string workspaceId, string userId, [Body] ClockifyMembership membership, CancellationToken cancellationToken = default);

    /// <summary>
    /// Activates a user in the workspace.
    /// Note: This endpoint is typically available only to workspace administrators.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID to activate.</param>
    /// <returns>The activated user's data.</returns>
    [Post("/workspaces/{workspaceId}/users/{userId}/activate")]
    Task<ClockifyUser> ActivateUserAsync(string workspaceId, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deactivates a user in the workspace.
    /// Note: This endpoint is typically available only to workspace administrators.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID to deactivate.</param>
    /// <returns>The deactivated user's data.</returns>
    [Post("/workspaces/{workspaceId}/users/{userId}/deactivate")]
    Task<ClockifyUser> DeactivateUserAsync(string workspaceId, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the hourly rate for a user in the workspace.
    /// Note: This endpoint is typically available only to workspace administrators.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID to set the rate for.</param>
    /// <param name="hourlyRate">The hourly rate data.</param>
    /// <returns>The updated user's data.</returns>
    [Post("/workspaces/{workspaceId}/users/{userId}/hourlyRate")]
    Task<ClockifyUser> SetUserHourlyRateAsync(string workspaceId, string userId, [Body] ClockifyHourlyRate hourlyRate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the cost rate for a user in the workspace.
    /// Note: This endpoint is typically available only to workspace administrators.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID to set the cost rate for.</param>
    /// <param name="costRate">The cost rate data.</param>
    /// <returns>The updated user's data.</returns>
    [Post("/workspaces/{workspaceId}/users/{userId}/costRate")]
    Task<ClockifyUser> SetUserCostRateAsync(string workspaceId, string userId, [Body] ClockifyHourlyRate costRate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Assigns roles to a user in the workspace.
    /// Note: This endpoint is typically available only to workspace administrators.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID to assign roles to.</param>
    /// <param name="roles">The array of roles to assign.</param>
    /// <returns>The updated user's data.</returns>
    [Put("/workspaces/{workspaceId}/users/{userId}/roles")]
    Task<ClockifyUser> AssignUserRolesAsync(string workspaceId, string userId, [Body] ClockifyRole[] roles, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes roles from a user in the workspace.
    /// Note: This endpoint is typically available only to workspace administrators.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID to remove roles from.</param>
    /// <param name="roles">The array of roles to remove.</param>
    /// <returns>The updated user's data.</returns>
    [Delete("/workspaces/{workspaceId}/users/{userId}/roles")]
    Task<ClockifyUser> RemoveUserRolesAsync(string workspaceId, string userId, [Body] ClockifyRole[] roles, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resends invitation to a pending user.
    /// Note: This endpoint is typically available only to workspace administrators.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID to resend invitation to.</param>
    /// <returns>Task representing the resend operation.</returns>
    [Post("/workspaces/{workspaceId}/users/{userId}/resendInvitation")]
    Task ResendUserInvitationAsync(string workspaceId, string userId, CancellationToken cancellationToken = default);
}