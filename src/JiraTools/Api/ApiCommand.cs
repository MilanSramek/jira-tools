using System.CommandLine;

using JiraTools.Api.Clockify;
using JiraTools.Api.Jira;

namespace JiraTools.Api;

internal sealed class ApiCommand : Command
{
    public ApiCommand(
        ApiJiraCommand apiJiraCommand,
        ApiClockifyCommand apiClockifyCommand)
        : base("api", "Manage connected API")
    {
        Add(apiJiraCommand);
        Add(apiClockifyCommand);
    }

}