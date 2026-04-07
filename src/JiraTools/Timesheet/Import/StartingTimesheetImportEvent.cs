using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed record StartingTimesheetImportEvent
(
    TimesheetImportSettings Settings
) 
    : INotification;