using FluentResults;
using MediatR;

namespace JiraTools.Timesheet.Import.Events;

internal sealed record ClockifyTimesheetAcquiringFailedEvent(IReadOnlyList<IError> Errors) : INotification;