using System.Text.Json.Serialization;

namespace Clockify;

public sealed record UpdateUserRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("memberships")]
    public ClockifyMembership[]? Memberships { get; init; }

    [JsonPropertyName("roles")]
    public ClockifyRole[]? Roles { get; init; }
}