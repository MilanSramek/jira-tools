using System.Text.Json.Serialization;

namespace Clockify;

public sealed record CreateUserRequest
{
    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("sendInvitation")]
    public bool SendInvitation { get; init; } = true;
}