using MediatR;
using Spectre.Console;

namespace JiraTools.Timesheet.Import;

internal sealed class TimesheetImporterEventsHandler :
    INotificationHandler<StartingTimesheetImportEvent>,
    INotificationHandler<ClockifyTimesheetAcquiredEvent>,
    INotificationHandler<JiraTimesheetAcquiredEvent>,
    INotificationHandler<MisalignedIssuesEvent>,
    INotificationHandler<TimesheetImportFinishedEvent>
{
    public Task Handle(StartingTimesheetImportEvent notification, CancellationToken _)
    {
        var settings = notification.Settings;
        AnsiConsole.MarkupLine(
            $"[dim]→[/]  Starting timesheet import in range [dim][[{settings.From:d}, {settings.To:d}]][/]");

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
                    started.HasValue ? started.Value.ToString("d") : string.Empty,
                    Markup.Escape(timeSpent.ToString()),
                    Markup.Escape(comment ?? string.Empty));
            }

            return table;
        }
    }

    public Task Handle(TimesheetImportFinishedEvent notification, CancellationToken _)
    {
        var count = notification.ImportedWorklogs.Count();
        AnsiConsole.MarkupLine($"[green]✓[/]  Timesheet import finished — [bold]{count}[/] worklogs imported");

        return Task.CompletedTask;
    }
}