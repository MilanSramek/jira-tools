using Jira;
using JiraTools.Extensions;
using Microsoft.Extensions.Options;

namespace JiraTools.Timesheet;

internal sealed class JiraTimesheetProvider
(
    IJiraIssueApi jiraIssueApi,
    IOptions<JiraTimesheetProviderOptions> options
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

        var worklogsResponses = await response.Issues
            .Select(issue => (Func<Task<JiraWorklogsResponse>>)(() => jiraIssueApi.GetIssueWorklogsAsync(
                issue.Key,
                startedAfter: from.ToDateTime(TimeOnly.MinValue),
                startedBefore: to.ToDateTime(TimeOnly.MaxValue))))
            .WhenAll(options.Value.MaxRequestParallelism);

        var issues = response.Issues.ToDictionary(issue => issue.Id);
        return worklogsResponses
            .SelectMany(_ => _.Worklogs)
            .Where(worklog => worklog.Author?.AccountId == accountId
                && issues.ContainsKey(worklog.IssueId!))
            .Select(worklog => new JiraTimesheetEntry(
                Worklog: worklog,
                Issue: issues[worklog.IssueId!]));
    }
}
