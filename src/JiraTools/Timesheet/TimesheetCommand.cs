using System.CommandLine;

using JiraTools.Timesheet.Import;

namespace JiraTools.Timesheet;

internal sealed class TimesheetCommand : Command
{
    public TimesheetCommand(ImportTimesheetCommand importTimesheetCommand)
        : base("timesheet", "Manage timesheets")
    {
        Subcommands.Add(importTimesheetCommand);
    }
}