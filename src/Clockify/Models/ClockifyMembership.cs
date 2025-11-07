using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyMembership
{
    [JsonPropertyName("userId")]
    public required string UserId { get; init; }

    [JsonPropertyName("hourlyRate")]
    public ClockifyHourlyRate? HourlyRate { get; init; }

    [JsonPropertyName("costRate")]
    public ClockifyHourlyRate? CostRate { get; init; }

    [JsonPropertyName("targetId")]
    public required string TargetId { get; init; }

    [JsonPropertyName("membershipType")]
    public required string MembershipType { get; init; }

    [JsonPropertyName("membershipStatus")]
    public required string MembershipStatus { get; init; }

    [JsonPropertyName("roles")]
    public ClockifyRole[]? Roles { get; init; }
}