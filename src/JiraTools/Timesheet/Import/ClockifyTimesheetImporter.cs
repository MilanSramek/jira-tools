using Clockify;

using Jira;

namespace JiraTools.Timesheet.Import;

internal sealed class ClockifyTimesheetImporter
(
    IClockifyUserApi clockifyUserApi,
    ClockifyTimesheetProvider clockifyTimesheetProvider,
    IJiraUserApi jiraUserApi,
    JiraTimesheetProvider jiraTimesheetProvider,
    IJiraIssueApi jiraIssueApi
) :
    ITimesheetImporter
{
    private readonly struct JiraIssueKey(string key)
    {
        private readonly string _key = key ?? throw new ArgumentNullException(nameof(key));

        public static JiraIssueKey FromClockifyTask(ClockifyProject project, ClockifyTaskName taskName)
            => new($"{project.Name}-{taskName.Key}");

        public override string ToString() => _key;
    }

    private readonly record struct ReconciliationResult
    (
        IEnumerable<(JiraIssueKey Key, CreateJiraWorklogRequest Request)> ToCreate,
        IEnumerable<JiraTimesheetEntry> ToDelete
    );

    public async Task ExecuteAsync(TimesheetImportSettings settings,
        CancellationToken cancellationToken)
    {
        var getClockifyTimesheetTask = GetClockifyCurrentUserTimesheetAsync(
            settings.From,
            settings.To,
            cancellationToken);
        var getJiraTimesheetTask = GetJiraCurrentUserTimesheetAsync(
            settings.From,
            settings.To,
            cancellationToken);
        await Task.WhenAll(getClockifyTimesheetTask, getJiraTimesheetTask);

        var clockifyTimesheet = getClockifyTimesheetTask.Result;
        var jiraTimesheet = getJiraTimesheetTask.Result;

        var (toCreate, toDelete) = ReconcileTimesheetEntries(clockifyTimesheet, jiraTimesheet);
        if (toDelete.Any())
        {
            Console.Error.WriteLine("Error: Import cannot proceed because some existing issues do not align.");
            foreach (var worklog in toDelete)
            {
                Console.Error.WriteLine(
                    $"- Issue: {worklog.Issue.Key}, "
                    + $"Started: {worklog.Worklog.Started:d}, "
                    + $"Time Spent: {worklog.Worklog.TimeSpent}, "
                    + $"Comment: {worklog.Worklog.Comment?.GetText() ?? "<no comment>"}");
            }
            Console.Out.WriteLine("Warning: The following worklogs will not be created:");
            foreach (var (Key, Request) in toCreate)
            {
                Console.Out.WriteLine(
                    $"- Issue: {Key}, "
                    + $"Started: {Request.Started:d}, "
                    + $"Time Spent: {Request.TimeSpent}, "
                    + $"Comment: {Request.Comment?.GetText() ?? "<no comment>"}");   
            }
            return;
            // throw new Exception("Import failed. Some existing issues do not align.");
        }

        var createJiraWorklogTasks = toCreate
            .Select(_ => jiraIssueApi.CreateWorklogAsync(
                _.Key.ToString(),
                _.Request,
                cancellationToken: cancellationToken));
        await Task.WhenAll(createJiraWorklogTasks);
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