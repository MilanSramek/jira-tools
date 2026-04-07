using Clockify;

using Jira;
using MediatR;

namespace JiraTools.Timesheet.Import;

internal sealed class TimesheetImporter
(
    IClockifyUserApi clockifyUserApi,
    ClockifyTimesheetProvider clockifyTimesheetProvider,
    IJiraUserApi jiraUserApi,
    JiraTimesheetProvider jiraTimesheetProvider,
    IJiraIssueApi jiraIssueApi,
    IPublisher eventPublisher
) :
    ITimesheetImporter
{    
    private readonly record struct ReconciliationResult
    (
        IEnumerable<(JiraIssueKey Key, CreateJiraWorklogRequest Request)> ToCreate,
        IEnumerable<JiraTimesheetEntry> ToDelete
    );

    public async Task ExecuteAsync(TimesheetImportSettings settings,
        CancellationToken cancellationToken)
    {
        await eventPublisher.Publish(new StartingTimesheetImportEvent(settings), cancellationToken);

        var getClockifyTimesheetTask = GetClockifyCurrentUserTimesheetAsync(
            settings.From,
            settings.To,
            cancellationToken);
        var getJiraTimesheetTask = GetJiraCurrentUserTimesheetAsync(
            settings.From,
            settings.To,
            cancellationToken);
        await Task.WhenAll(getClockifyTimesheetTask, getJiraTimesheetTask);

        var clockifyTimesheet = getClockifyTimesheetTask.Result.ToArray();
        await eventPublisher.Publish(new ClockifyTimesheetAcquiredEvent(clockifyTimesheet), 
            cancellationToken);

        var jiraTimesheet = getJiraTimesheetTask.Result.ToArray();
        await eventPublisher.Publish(new JiraTimesheetAcquiredEvent(jiraTimesheet), cancellationToken);

        var (toCreate, toDelete) = ReconcileTimesheetEntries(clockifyTimesheet, jiraTimesheet);
        if (toDelete.Any())
        {
            await eventPublisher.Publish(new MisalignedIssuesEvent(toDelete, toCreate), 
                cancellationToken);
            return;
        }

        var createJiraWorklogTasks = toCreate
            .Select(_ => jiraIssueApi.CreateWorklogAsync(
                _.Key.ToString(),
                _.Request,
                cancellationToken: cancellationToken));
        var createdWorklogs = await Task.WhenAll(createJiraWorklogTasks);

        await eventPublisher.Publish(new TimesheetImportFinishedEvent(createdWorklogs),
            cancellationToken);
    }

    private static ReconciliationResult ReconcileTimesheetEntries(
        IEnumerable<ClockifyTimesheetEntry> clockifyTimesheet,
        IEnumerable<JiraTimesheetEntry> jiraTimesheet)
    {
        Dictionary<(JiraIssueKey, string?, DateOnly, TimeSpan TimeSpent), JiraTimesheetEntry> jiraEntries = jiraTimesheet
            .ToDictionary(entry => 
            (
                new JiraIssueKey(entry.Issue.Key),
                entry.Worklog.Comment?.GetText(),
                DateOnly.FromDateTime(entry.Worklog.Started!.Value.Date),
                entry.Worklog.TimeSpent
            ));

        var toCreate = new List<(JiraIssueKey, CreateJiraWorklogRequest)>();
        foreach (var clockifyEntry in clockifyTimesheet)
        {
            var clockifyEntryName = ClockifyTaskName.Parse(clockifyEntry.Task!.Name).Value;
            var jiraIssueKey = JiraIssueKey.FromClockifyTask(
                clockifyEntry.Project,
                clockifyEntryName);
            var timeSpent = clockifyEntry.TimeEntry.TimeInterval.End!.Value
                - clockifyEntry.TimeEntry.TimeInterval.Start;
            var description = !string.IsNullOrWhiteSpace(clockifyEntry.TimeEntry.Description)
                ? clockifyEntry.TimeEntry.Description.Trim()
                : clockifyEntryName.Hint?.Trim();

            var entryKey =
            (
                jiraIssueKey,
                description,
                DateOnly.FromDateTime(clockifyEntry.TimeEntry.TimeInterval.Start.Date),
                timeSpent
            );
            if (jiraEntries.Remove(entryKey, out var jiraEntry))
            {
                continue;
            }

            toCreate.Add((jiraIssueKey, new CreateJiraWorklogRequest
            {
                Comment = description is { }
                    ? JiraAdfDocument.CreateText(description)
                    : null,
                Started = clockifyEntry.TimeEntry.TimeInterval.Start,
                TimeSpent = timeSpent
            }));
        }

        return new ReconciliationResult
        {
            ToCreate = toCreate,
            ToDelete = jiraEntries.Values
        };
    }

    private async Task<IEnumerable<ClockifyTimesheetEntry>> GetClockifyCurrentUserTimesheetAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var currentUser = await clockifyUserApi.GetCurrentUserAsync(cancellationToken);
        return await clockifyTimesheetProvider.GetForUserAsync(
            currentUser.Id,
            currentUser.ActiveWorkspace ?? currentUser.DefaultWorkspace!,
            from,
            to,
            cancellationToken);
    }

    private async Task<IEnumerable<JiraTimesheetEntry>> GetJiraCurrentUserTimesheetAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var currentUser = await jiraUserApi.GetCurrentUserAsync(cancellationToken: cancellationToken);
        return await jiraTimesheetProvider.GetForUserAsync(
            currentUser.AccountId!,
            from,
            to,
            cancellationToken);
    }
}