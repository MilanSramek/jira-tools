namespace JiraTools.Timesheet.Import;

internal sealed record TimesheetImportSettings
(
    DateOnly From,
    DateOnly To
);