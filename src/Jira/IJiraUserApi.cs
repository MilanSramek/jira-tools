using Refit;

namespace Jira;

/// <summary>
/// Interface for Jira User REST API operations.
/// Provides methods to retrieve information about Jira users.
/// </summary>
public interface IJiraUserApi
{
    /// <summary>
    /// Gets information about the currently authenticated user.
    /// </summary>
    /// <param name="expand">Use expand to include additional information about the user in the response. Options include: groups, applicationRoles.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The currently authenticated user's information.</returns>
    [Get("/rest/api/3/myself")]
    Task<JiraUser> GetCurrentUserAsync(
        [AliasAs("expand")] string? expand = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a user by account ID.
    /// </summary>
    /// <param name="accountId">The account ID of the user.</param>
    /// <param name="expand">Use expand to include additional information about the user in the response. Options include: groups, applicationRoles.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user's information.</returns>
    [Get("/rest/api/3/user")]
    Task<JiraUser> GetUserByAccountIdAsync(
        [AliasAs("accountId")] string accountId,
        [AliasAs("expand")] string? expand = null,
        CancellationToken cancellationToken = default);
}
