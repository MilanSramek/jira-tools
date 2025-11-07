using System.CommandLine;

using JiraTools.Extensions;

namespace JiraTools.Timesheet.Import;

internal sealed class ImportTimesheetCommand : Command
{
    private readonly ClockifyTimesheetImporter _clockifyTimesheetImport;
    private readonly TimeProvider _timeProvider;

    public ImportTimesheetCommand(
        ClockifyTimesheetImporter clockifyTimesheetImport,
        TimeProvider timeProvider)
        : base("import", "Import timesheet data to Jira")
    {
        _clockifyTimesheetImport = clockifyTimesheetImport
            ?? throw new ArgumentNullException(nameof(clockifyTimesheetImport));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

        Setup();
    }

    private void Setup()
    {
        Option<TimesheetImportSource> timesheetSourceOption = new("--source", "-s")
        {
            Description = "Source of timesheet to import",
            Required = true,
        };
        timesheetSourceOption.AcceptOnlyFromAmong(typeof(TimesheetImportSource).GetEnumNames());
        Options.Add(timesheetSourceOption);

        Option<TimesheetImportTimePeriod> importPeriodOption = new("--period", "-p")
        {
            Description = "Period to import",
            Required = true,
        };
        importPeriodOption.AcceptOnlyFromAmong(typeof(TimesheetImportTimePeriod).GetEnumNames());
        Options.Add(importPeriodOption);

        TreatUnmatchedTokensAsErrors = true;

        SetAction((context, cancellationToken) =>
        {
            var timesheetSource = context.GetRequiredValue(timesheetSourceOption);
            var importPeriod = context.GetRequiredValue(importPeriodOption);
            return ExecuteAsync(timesheetSource, importPeriod, cancellationToken);
        });
    }

    private Task ExecuteAsync(
        TimesheetImportSource timesheetSource,
        TimesheetImportTimePeriod importTimePeriod,
        CancellationToken cancellationToken)
    {
        var timesheetImporter = timesheetSource switch
        {
            TimesheetImportSource.Clockify => _clockifyTimesheetImport,
            _ => throw new NotSupportedException($"Timesheet source '{timesheetSource}' is not supported.")
        };

        var (importFrom, importTo) = importTimePeriod switch
        {
            TimesheetImportTimePeriod.Day => _timeProvider.GetCurrentDayDateRange(),
            TimesheetImportTimePeriod.Week => _timeProvider.GetCurrentWeekDateRange(),
            TimesheetImportTimePeriod.Month => _timeProvider.GetCurrentMonthDateRange(),
            _ => throw new NotSupportedException("Unsupported period")
        };

        return timesheetImporter.ExecuteAsync(
            new TimesheetImportSettings(
                From: importFrom,
                To: importTo),
            cancellationToken);
    }
};