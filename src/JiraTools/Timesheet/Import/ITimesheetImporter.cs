namespace JiraTools.Timesheet.Import;

internal interface ITimesheetImporter
{
    Task ExecuteAsync(TimesheetImportSettings settings, CancellationToken cancellationToken);
}