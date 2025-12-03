namespace Clockify;

public static class ClockifyTimeEntryApiExtensions
{
    public static async Task<ClockifyTimeEntry[]> GetAllTimeEntriesAsync(
        this IClockifyTimeEntryApi timeEntryApi,
        string workspaceId,
        string userId,
        string? description = null,
        DateTime? start = null,
        DateTime? end = null,
        string? project = null,
        string? task = null,
        string? tags = null,
        bool? projectRequired = null,
        bool? taskRequired = null,
        bool? considerDurationFormat = null,
        bool? hydrated = null,
        bool? inProgress = null,
        CancellationToken cancellationToken = default)
    {
        const int pageSize = 200;
        List<ClockifyTimeEntry> allEntries = [];

        ClockifyTimeEntry[] timeEntriesPage;
        int page = 1;
        do
        {
            timeEntriesPage = await timeEntryApi.GetTimeEntriesAsync(
                workspaceId: workspaceId,
                userId: userId,
                description: description,
                start: start,
                end: end,
                project: project,
                task: task,
                tags: tags,
                projectRequired: projectRequired,
                taskRequired: taskRequired,
                considerDurationFormat: considerDurationFormat,
                hydrated: hydrated,
                inProgress: inProgress,
                page: page,
                pageSize: pageSize,
                cancellationToken: cancellationToken);
            allEntries.AddRange(timeEntriesPage);
            page++;
        } 
        while (timeEntriesPage.Length >= pageSize);

        return [.. allEntries];
    }
}