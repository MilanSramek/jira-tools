using MediatR;

namespace JiraTools.Timesheet.Import.Events;

internal sealed record ClockifyTimesheetAcquiredEvent
(
    IEnumerable<ClockifyTimesheetEntry> Timesheet
) 
    : INotification;