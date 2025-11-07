using System.CommandLine;
using JiraTools.Api;
using JiraTools.Timesheet;

namespace JiraTools;

internal sealed class JiraToolsRootCommand : RootCommand
{
    public JiraToolsRootCommand(
        TimesheetCommand importCommand,
        ApiCommand apiKeyCommand)
        : base("A set of tools to work with Jira")
    {
        Add(importCommand);
        Add(apiKeyCommand);
    }
}