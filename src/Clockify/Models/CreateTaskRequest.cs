using System.Text.Json.Serialization;

namespace Clockify;

public sealed record CreateTaskRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("assigneeIds")]
    public string[]? AssigneeIds { get; init; }

    [JsonPropertyName("estimate")]
    public string? Estimate { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; init; } = "ACTIVE";

    [JsonPropertyName("billable")]
    public bool? Billable { get; init; }
}