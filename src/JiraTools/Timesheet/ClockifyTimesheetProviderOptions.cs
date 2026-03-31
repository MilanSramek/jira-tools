namespace JiraTools.Timesheet;

internal sealed record ClockifyTimesheetProviderOptions
{
    public int? MaxRequestParallelism { get; init; }
}