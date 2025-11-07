using System.CommandLine;

using JiraTools.Api.Clockify.Set;

namespace JiraTools.Api.Clockify;

internal sealed class ApiClockifyCommand : Command
{
    public ApiClockifyCommand(ApiClockifySetCommand setCommand)
        : base("Clockify", "Manage Clockify connection")
    {
        Add(setCommand);
    }
}