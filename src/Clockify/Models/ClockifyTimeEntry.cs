using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyTimeEntry
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("tagIds")]
    public string[]? TagIds { get; init; }

    [JsonPropertyName("userId")]
    public required string UserId { get; init; }

    [JsonPropertyName("billable")]
    public bool Billable { get; init; }

    [JsonPropertyName("taskId")]
    public string? TaskId { get; init; }

    [JsonPropertyName("projectId")]
    public required string ProjectId { get; init; }

    [JsonPropertyName("timeInterval")]
    public required ClockifyTimeInterval TimeInterval { get; init; }

    [JsonPropertyName("workspaceId")]
    public required string WorkspaceId { get; init; }

    [JsonPropertyName("hourlyRate")]
    public ClockifyHourlyRate? HourlyRate { get; init; }

    [JsonPropertyName("costRate")]
    public ClockifyHourlyRate? CostRate { get; init; }

    [JsonPropertyName("user")]
    public ClockifyUser? User { get; init; }

    [JsonPropertyName("project")]
    public ClockifyProject? Project { get; init; }

    [JsonPropertyName("task")]
    public ClockifyTask? Task { get; init; }

    [JsonPropertyName("tags")]
    public ClockifyTag[]? Tags { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("kioskId")]
    public string? KioskId { get; init; }

    [JsonPropertyName("isLocked")]
    public bool IsLocked { get; init; }

    [JsonPropertyName("customFieldValues")]
    public ClockifyCustomFieldValue[]? CustomFieldValues { get; init; }
}