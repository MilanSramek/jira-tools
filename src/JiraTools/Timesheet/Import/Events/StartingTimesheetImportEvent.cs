using MediatR;

namespace JiraTools.Timesheet.Import.Events;

internal sealed record StartingTimesheetImportEvent
(
    TimesheetImportSettings Settings
) 
    : INotification;