using FluentResults;
using MediatR;

namespace JiraTools.Timesheet.Import.Events;

internal sealed record JiraTimesheetAcquiringFailedEvent(IReadOnlyList<IError> Errors) : INotification;