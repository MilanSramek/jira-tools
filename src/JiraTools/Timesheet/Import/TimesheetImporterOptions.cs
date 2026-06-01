namespace JiraTools.Timesheet.Import;

internal sealed record TimesheetImporterOptions
{
    public int? JiraMaxRequestParallelism { get; set; }
}