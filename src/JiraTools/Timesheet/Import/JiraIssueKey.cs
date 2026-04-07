using Clockify;

namespace JiraTools.Timesheet.Import;

internal readonly struct JiraIssueKey(string key)
{
    private readonly string _key = key ?? throw new ArgumentNullException(nameof(key));

    public static JiraIssueKey FromClockifyTask(ClockifyProject project, ClockifyTaskName taskName)
        => new($"{project.Name}-{taskName.Key}");

    public override string ToString() => _key;
}