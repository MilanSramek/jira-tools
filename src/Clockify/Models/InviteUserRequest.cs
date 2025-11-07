using System.Text.Json.Serialization;

namespace Clockify;

public sealed record InviteUserRequest
{
    [JsonPropertyName("emails")]
    public required string[] Emails { get; init; }

    [JsonPropertyName("memberships")]
    public ClockifyMembership[]? Memberships { get; init; }
}