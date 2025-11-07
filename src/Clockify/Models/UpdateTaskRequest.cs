using System.Text.Json.Serialization;

namespace Clockify;

public sealed record UpdateTaskRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("assigneeIds")]
    public string[]? AssigneeIds { get; init; }

    [JsonPropertyName("estimate")]
    public string? Estimate { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }

    [JsonPropertyName("billable")]
    public bool? Billable { get; init; }
}