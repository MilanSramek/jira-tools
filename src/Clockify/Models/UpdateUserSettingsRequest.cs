using System.Text.Json.Serialization;

namespace Clockify;

public sealed record UpdateUserSettingsRequest
{
    [JsonPropertyName("weekStart")]
    public string? WeekStart { get; init; }

    [JsonPropertyName("timeZone")]
    public string? TimeZone { get; init; }

    [JsonPropertyName("timeFormat")]
    public string? TimeFormat { get; init; }

    [JsonPropertyName("dateFormat")]
    public string? DateFormat { get; init; }

    [JsonPropertyName("sendNewsletter")]
    public bool? SendNewsletter { get; init; }

    [JsonPropertyName("weeklyUpdates")]
    public bool? WeeklyUpdates { get; init; }

    [JsonPropertyName("longRunning")]
    public bool? LongRunning { get; init; }

    [JsonPropertyName("timeTrackingManual")]
    public bool? TimeTrackingManual { get; init; }

    [JsonPropertyName("isCompactViewOn")]
    public bool? IsCompactViewOn { get; init; }

    [JsonPropertyName("dashboardSelection")]
    public string? DashboardSelection { get; init; }

    [JsonPropertyName("dashboardViewType")]
    public string? DashboardViewType { get; init; }

    [JsonPropertyName("dashboardPinToTop")]
    public bool? DashboardPinToTop { get; init; }

    [JsonPropertyName("projectListCollapse")]
    public int? ProjectListCollapse { get; init; }

    [JsonPropertyName("collapseAllProjectLists")]
    public bool? CollapseAllProjectLists { get; init; }

    [JsonPropertyName("groupSimilarEntriesDisabled")]
    public bool? GroupSimilarEntriesDisabled { get; init; }

    [JsonPropertyName("myStartOfDay")]
    public string? MyStartOfDay { get; init; }

    [JsonPropertyName("projectPickerTaskFilter")]
    public bool? ProjectPickerTaskFilter { get; init; }

    [JsonPropertyName("lang")]
    public string? Language { get; init; }

    [JsonPropertyName("theme")]
    public string? Theme { get; init; }
}