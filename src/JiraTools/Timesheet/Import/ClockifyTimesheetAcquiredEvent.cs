using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed record ClockifyTimesheetAcquiredEvent
(
    IEnumerable<ClockifyTimesheetEntry> Timesheet
) 
    : INotification;