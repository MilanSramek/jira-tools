using System.CommandLine;

using JiraTools.Api.Jira.Set;

namespace JiraTools.Api.Jira;

internal sealed class ApiJiraCommand : Command
{
    public ApiJiraCommand(ApiJiraSetCommand setCommand) : base("Jira", "Manage Jira connection")
    {
        Add(setCommand);
    }
}