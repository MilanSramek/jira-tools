using Clockify;

using FluentResults;

using Jira;
using JiraTools.Extensions;
using MediatR;
using Microsoft.Extensions.Options;
using Refit;

namespace JiraTools.Timesheet.Import;

internal sealed class TimesheetImporter
(
    IClockifyUserApi clockifyUserApi,
    ClockifyTimesheetProvider clockifyTimesheetProvider,
    IJiraUserApi jiraUserApi,
    JiraTimesheetProvider jiraTimesheetProvider,
    IJiraIssueApi jiraIssueApi,
    IPublisher eventPublisher,
    IOptions<TimesheetImporterOptions> options
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
        try
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
            if (getClockifyTimesheetTask.Result.IsFailed)
            {
                await eventPublisher.Publish(
                    new ClockifyTimesheetAcquiringFailedEvent(getClockifyTimesheetTask.Result.Errors),
                    cancellationToken);
            }
            if (getJiraTimesheetTask.Result.IsFailed)
            {
                await eventPublisher.Publish(
                    new JiraTimesheetAcquiringFailedEvent(getJiraTimesheetTask.Result.Errors),
                    cancellationToken);
            }
            if (getClockifyTimesheetTask.Result.IsFailed || getJiraTimesheetTask.Result.IsFailed)
            {
                return;
            }

            var clockifyTimesheet = getClockifyTimesheetTask.Result.Value.ToArray();
            await eventPublisher.Publish(new ClockifyTimesheetAcquiredEvent(clockifyTimesheet),
                cancellationToken);

            var jiraTimesheet = getJiraTimesheetTask.Result.Value.ToArray();
            await eventPublisher.Publish(new JiraTimesheetAcquiredEvent(jiraTimesheet), cancellationToken);

            var (toCreate, toDelete) = ReconcileTimesheetEntries(clockifyTimesheet, jiraTimesheet);
            if (toDelete.Any())
            {
                await eventPublisher.Publish(new MisalignedIssuesEvent(toDelete, toCreate),
                    cancellationToken);
                return;
            }

            var createJiraWorklogResult = await CreateJiraWorklogAsync(toCreate, cancellationToken);
            if (createJiraWorklogResult.IsFailed)
            {
                await eventPublisher.Publish(new JiraTimesheetPublishingFailedEvent(createJiraWorklogResult.Errors),
                    cancellationToken);
                return;
            }
            await eventPublisher.Publish(new TimesheetImportFinishedEvent(createJiraWorklogResult.Value),
            cancellationToken);
        }
        catch (Exception ex)
        {
            await eventPublisher.Publish(new TimesheetImportFailedEvent(ex), cancellationToken);
        }
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

    private async Task<Result<IEnumerable<ClockifyTimesheetEntry>>> GetClockifyCurrentUserTimesheetAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = await clockifyUserApi.GetCurrentUserAsync(cancellationToken);
            return Result.Ok(await clockifyTimesheetProvider.GetForUserAsync(
                currentUser.Id,
                currentUser.ActiveWorkspace ?? currentUser.DefaultWorkspace!,
                from,
                to,
                cancellationToken));
        }
        catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Result.Fail("Unauthorized access to Clockify API");
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError(ex));
        }
    }

    private async Task<Result<IEnumerable<JiraTimesheetEntry>>> GetJiraCurrentUserTimesheetAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = await jiraUserApi.GetCurrentUserAsync(cancellationToken: cancellationToken);
            return Result.Ok(await jiraTimesheetProvider.GetForUserAsync(
                currentUser.AccountId!,
                from,
                to,
                cancellationToken));
        }
        catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Result.Fail("Unauthorized access to Jira API");
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError(ex));
        }
    }

    private async Task<Result<JiraWorklog[]>> CreateJiraWorklogAsync(
        IEnumerable<(JiraIssueKey Key, CreateJiraWorklogRequest Request)> toCreate,
        CancellationToken cancellationToken)
    {
        try
        {
            var createJiraWorklogTasks = toCreate
                .Select(_ => 
                {
                    return (Func<Task<JiraWorklog>>)(() => jiraIssueApi.CreateWorklogAsync(
                        _.Key.ToString(),
                        _.Request,
                        cancellationToken: cancellationToken));
                });
            var createdWorklogs = await createJiraWorklogTasks.WhenAll(options.Value.JiraMaxRequestParallelism);
            return Result.Ok(createdWorklogs);
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError(ex));
        }
    }
}