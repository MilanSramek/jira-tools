using Jira;

namespace JiraTools.Timesheet;

internal sealed class JiraTimesheetProvider
(
    IJiraIssueApi jiraIssueApi
)
{
    public async Task<IEnumerable<JiraTimesheetEntry>> GetForUserAsync(
        string accountId,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var query = $"worklogAuthor = {accountId} AND worklogDate >= {from:yyyy-MM-dd} AND worklogDate <= {to:yyyy-MM-dd}";
        JiraSearchResponse response = await jiraIssueApi.SearchIssuesAsync(
            jql: query,
            fields: "key",
            maxResults: 1000,
            cancellationToken: cancellationToken);

        var getWorklogTasks = response.Issues
            .Select(issue => jiraIssueApi.GetIssueWorklogsAsync(
                issue.Key,
                startedAfter: from.ToDateTime(TimeOnly.MinValue),
                startedBefore: to.ToDateTime(TimeOnly.MaxValue)));

        var issues = response.Issues.ToDictionary(issue => issue.Id);

        await Task.WhenAll(getWorklogTasks);

        return getWorklogTasks
            .SelectMany(task => task.Result.Worklogs)
            .Where(worklog => worklog.Author?.AccountId == accountId
                && issues.ContainsKey(worklog.IssueId!))
            .Select(worklog => new JiraTimesheetEntry(
                Worklog: worklog,
                Issue: issues[worklog.IssueId!]));
    }
}
