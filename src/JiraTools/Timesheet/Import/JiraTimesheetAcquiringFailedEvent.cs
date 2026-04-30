using FluentResults;
using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed record JiraTimesheetAcquiringFailedEvent(IReadOnlyList<IError> Errors) : INotification;