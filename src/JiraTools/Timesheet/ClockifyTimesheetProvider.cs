using Clockify;

namespace JiraTools.Timesheet;

internal sealed class ClockifyTimesheetProvider
(
    IClockifyTimeEntryApi timeEntryApi,
    IClockifyProjectApi projectApi,
    IClockifyTaskApi taskApi
)
{
    public async Task<IEnumerable<ClockifyTimesheetEntry>> GetForUserAsync(
        string userId,
        string workspaceId,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        ClockifyTimeEntry[] timeEntries = await timeEntryApi.GetAllTimeEntriesAsync(
            workspaceId: workspaceId,
            userId: userId,
            start: from.ToDateTime(TimeOnly.MinValue),
            end: to.ToDateTime(TimeOnly.MaxValue),
            cancellationToken: cancellationToken);

        var projectIds = timeEntries
           .Select(_ => _.ProjectId)
           .OfType<string>()
           .Distinct()
           .ToList();

        Task<ClockifyProject[]> getProjectsTask = Task.WhenAll(projectIds
            .Select(projectId => projectApi.GetProjectByIdAsync(
                workspaceId: workspaceId,
                projectId: projectId,
                cancellationToken: cancellationToken)));
        Task<ClockifyTask[]> getTasksTask = Task.WhenAll(timeEntries
           .Select(_ => (_.ProjectId, _.TaskId))
           .OfType<(string ProjectId, string TaskId)>()
           .Distinct()
           .Select(_ => taskApi.GetTaskByIdAsync(
                workspaceId: workspaceId,
                projectId: _.ProjectId,
                taskId: _.TaskId,
                cancellationToken: cancellationToken)));

        await Task.WhenAll(getProjectsTask, getTasksTask);

        var tasks = getTasksTask.Result
            .ToDictionary(task => (task.ProjectId, task.Id));
        var projects = getProjectsTask.Result
            .ToDictionary(project => project.Id);

        return timeEntries
            .Select(timeEntry =>
            {
                var project = projects[timeEntry.ProjectId];
                var task = timeEntry is { TaskId: string taskId }
                        ? tasks[(timeEntry.ProjectId, taskId)]
                        : null;
                return new ClockifyTimesheetEntry(timeEntry, project, task);
            });
    }
}
