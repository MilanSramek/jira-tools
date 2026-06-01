using FluentResults;
using JiraTools.Timesheet.Import.Events;
using MediatR;
using Refit;
using Spectre.Console;

namespace JiraTools.Timesheet.Import;

internal sealed class TimesheetImporterEventsHandler :
    INotificationHandler<StartingTimesheetImportEvent>,
    INotificationHandler<ClockifyTimesheetAcquiredEvent>,
    INotificationHandler<JiraTimesheetAcquiredEvent>,
    INotificationHandler<MisalignedIssuesEvent>,
    INotificationHandler<TimesheetImportFinishedEvent>,
    INotificationHandler<TimesheetImportFailedEvent>,
    INotificationHandler<JiraTimesheetAcquiringFailedEvent>,
    INotificationHandler<ClockifyTimesheetAcquiringFailedEvent>,
    INotificationHandler<JiraTimesheetPublishingFailedEvent>
{
    private readonly TaskCompletionSource _importFinishedCompletionSource = new();

    public Task Handle(StartingTimesheetImportEvent notification, CancellationToken _)
    {
        var settings = notification.Settings;
        AnsiConsole.MarkupLine($"[dim]→[/]  Starting timesheet import in range [dim][[{settings.From:d}, {settings.To:d}]][/]");

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Default)
            .SpinnerStyle(Style.Parse("green"))
            .StartAsync("Importing...", _ => _importFinishedCompletionSource.Task);

        return Task.CompletedTask;
    }

    public Task Handle(ClockifyTimesheetAcquiredEvent notification, CancellationToken _)
    {
        var count = notification.Timesheet.Count();
        AnsiConsole.MarkupLine($"[green]✓[/]  Clockify timesheet acquired — [bold]{count}[/] entries");

        return Task.CompletedTask;
    }

    public Task Handle(JiraTimesheetAcquiredEvent notification, CancellationToken _)
    {
        var count = notification.Timesheet.Count();
        AnsiConsole.MarkupLine($"[green]✓[/]  Jira timesheet acquired — [bold]{count}[/] entries");

        return Task.CompletedTask;
    }

    public Task Handle(MisalignedIssuesEvent notification, CancellationToken _)
    {
        _importFinishedCompletionSource.SetResult();

        AnsiConsole.MarkupLine("[red]✗[/]  Import cannot proceed — misaligned issues detected");
        var misalignedJiraIssuesTable = BuildWorklogTable(
            notification.MisalignedJiraIssues.Select(entry => (
                entry.Issue.Key,
                entry.Worklog.Started,
                entry.Worklog.TimeSpent,
                Comment: entry.Worklog.Comment?.GetText())))
            .BorderColor(Color.Red);

        AnsiConsole.Write(misalignedJiraIssuesTable);

        AnsiConsole.MarkupLine("[yellow]⚠[/]  The following worklogs will not be created");
        var notCreatedWorklogsTable = BuildWorklogTable(
            notification.NotCreatedEntries.Select(entry => (
                Key: entry.Key.ToString(),
                Started: (DateTime?)entry.Request.Started,
                entry.Request.TimeSpent,
                Comment: entry.Request.Comment?.GetText())))
            .BorderColor(Color.Yellow);

        AnsiConsole.Write(notCreatedWorklogsTable);

        return Task.CompletedTask;

        static Table BuildWorklogTable(IEnumerable<(string Key, DateTime? Started, TimeSpan TimeSpent, string? Comment)> rows)
        {
            var table = new Table()
                .AddColumn("[bold]Issue[/]")
                .AddColumn("[bold]Started[/]")
                .AddColumn("[bold]Time Spent[/]")
                .AddColumn("[bold]Comment[/]");

            foreach (var (key, started, timeSpent, comment) in rows)
            {
                table.AddRow(
                    Markup.Escape(key),
                    started.HasValue ? started.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                    Markup.Escape(timeSpent.ToString()),
                    Markup.Escape(comment ?? string.Empty));
            }

            return table;
        }
    }

    public Task Handle(TimesheetImportFinishedEvent notification, CancellationToken _)
    {
        _importFinishedCompletionSource.SetResult();

        var count = notification.ImportedWorklogs.Count();
        AnsiConsole.MarkupLine($"[green]✓[/]  Timesheet import finished — [bold]{count}[/] worklogs imported");

        return Task.CompletedTask;
    }

    public Task Handle(TimesheetImportFailedEvent notification, CancellationToken cancellationToken)
    {
        _importFinishedCompletionSource.SetResult();

        AnsiConsole.MarkupLine("[red]✗[/]  Timesheet import failed");
        AnsiConsole.MarkupLine($"[red]✗[/]  Reason: {Markup.Escape(notification.Error.Message)}");

        return Task.CompletedTask;
    }

    public Task Handle(JiraTimesheetAcquiringFailedEvent notification, CancellationToken cancellationToken)
    {
        _importFinishedCompletionSource.SetResult();

        WriteFailure("Jira", "timesheet acquiring failed", notification.Errors);

        return Task.CompletedTask;
    }

    public Task Handle(ClockifyTimesheetAcquiringFailedEvent notification, CancellationToken cancellationToken)
    {
        _importFinishedCompletionSource.SetResult();

        WriteFailure("Clockify", "timesheet acquiring failed", notification.Errors);

        return Task.CompletedTask;
    }

    public Task Handle(JiraTimesheetPublishingFailedEvent notification, CancellationToken cancellationToken)
    {
        _importFinishedCompletionSource.SetResult();

        WriteFailure("Jira", "timesheet creation failed", notification.Errors);

        return Task.CompletedTask;
    }

    private static void WriteFailure(string source, string failure, IEnumerable<IError> errors)
    {
        var rows = errors
            .SelectMany(GetErrorMessages)
            .Select(Markup.Escape)
            .ToArray();

        var reasonLines = rows.Length == 0
            ? new Markup("[grey]- no reason provided[/]")
            : new Markup(string.Join(Environment.NewLine, rows.Select(row => $"[red]-[/] {row}")));

        var body = new Rows(
            new Markup($"[red]✗[/]  [bold]{Markup.Escape(source)}[/] {Markup.Escape(failure)}"),
            new Rule("[grey]Captured errors[/]").RuleStyle("red"),
            reasonLines);

        var panel = new Panel(body)
            .Header("[bold red]Import Failure[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(Style.Parse("red"))
            .Padding(1, 0, 1, 0)
            .Expand();

        AnsiConsole.Write(panel);
    }

    private static IEnumerable<string> GetErrorMessages(IError error)
    {
        if (error is ExceptionalError { Exception: { } exception })
        {
            var exceptionMessages = GetExceptionMessages(exception)
                .Where(message => !string.IsNullOrWhiteSpace(message))
                .Distinct()
                .ToArray();

            if (exceptionMessages.Length > 0)
            {
                foreach (var message in exceptionMessages)
                {
                    yield return message;
                }

                yield break;
            }
        }

        if (!string.IsNullOrWhiteSpace(error.Message))
        {
            yield return error.Message;
        }

        static IEnumerable<string> GetExceptionMessages(Exception exception)
        {
            switch (exception)
            {
                case AggregateException aggregateException:
                    foreach (var aggregateInnerException in aggregateException.Flatten().InnerExceptions)
                    {
                        foreach (var message in GetExceptionMessages(aggregateInnerException))
                        {
                            yield return message;
                        }
                    }
                    yield break;
                case ApiException apiException:
                    if (!string.IsNullOrWhiteSpace(apiException.Content))
                    {
                        yield return !string.IsNullOrWhiteSpace(apiException.Message)
                            ? $"{apiException.Message}{Environment.NewLine}  {apiException.Content}"
                            : apiException.Content;
                        yield break;
                    }

                    if (!string.IsNullOrWhiteSpace(apiException.Message))
                    {
                        yield return apiException.Message;
                    }
                    yield break;
                default:
                    if (!string.IsNullOrWhiteSpace(exception.Message))
                    {
                        yield return exception.Message;
                    }

                    if (exception.InnerException is { } innerException)
                    {
                        foreach (var message in GetExceptionMessages(innerException))
                        {
                            yield return message;
                        }
                    }
                    yield break;
            }
        }
    }
}