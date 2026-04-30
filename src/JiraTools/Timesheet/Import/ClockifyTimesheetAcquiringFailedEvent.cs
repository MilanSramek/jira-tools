using FluentResults;
using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed record ClockifyTimesheetAcquiringFailedEvent(IReadOnlyList<IError> Errors) : INotification;