using System.Text.Json.Serialization;

namespace Clockify;

public sealed record ClockifyProject
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("hourlyRate")]
    public ClockifyHourlyRate? HourlyRate { get; init; }

    [JsonPropertyName("clientId")]
    public string? ClientId { get; init; }

    [JsonPropertyName("workspaceId")]
    public required string WorkspaceId { get; init; }

    [JsonPropertyName("billable")]
    public bool Billable { get; init; }

    [JsonPropertyName("memberships")]
    public ClockifyProjectMembership[]? Memberships { get; init; }

    [JsonPropertyName("color")]
    public required string Color { get; init; }

    [JsonPropertyName("estimate")]
    public ClockifyProjectEstimate? Estimate { get; init; }

    [JsonPropertyName("archived")]
    public bool Archived { get; init; }

    [JsonPropertyName("duration")]
    public string? Duration { get; init; }

    [JsonPropertyName("clientName")]
    public string? ClientName { get; init; }

    [JsonPropertyName("note")]
    public string? Note { get; init; }

    [JsonPropertyName("template")]
    public bool Template { get; init; }

    [JsonPropertyName("public")]
    public bool Public { get; init; }

    [JsonPropertyName("budgetEstimate")]
    public ClockifyBudgetEstimate? BudgetEstimate { get; init; }

    [JsonPropertyName("timeEstimate")]
    public ClockifyTimeEstimate? TimeEstimate { get; init; }

    [JsonPropertyName("tasks")]
    public ClockifyTask[]? Tasks { get; init; }
}