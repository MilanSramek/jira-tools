using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyUser
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("profilePicture")]
    public string? ProfilePicture { get; init; }

    [JsonPropertyName("activeWorkspace")]
    public string? ActiveWorkspace { get; init; }

    [JsonPropertyName("defaultWorkspace")]
    public string? DefaultWorkspace { get; init; }

    [JsonPropertyName("settings")]
    public ClockifyUserSettings? Settings { get; init; }

    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [JsonPropertyName("memberships")]
    public ClockifyMembership[]? Memberships { get; init; }

    [JsonPropertyName("roles")]
    public ClockifyRole[]? Roles { get; init; }
}
