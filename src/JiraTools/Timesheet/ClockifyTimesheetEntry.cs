using Clockify;

namespace JiraTools.Timesheet;

internal sealed record ClockifyTimesheetEntry
(
    ClockifyTimeEntry TimeEntry,
    ClockifyProject Project,
    ClockifyTask? Task
);