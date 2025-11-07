using CSharpFunctionalExtensions;

using JiraTools.Timesheet.Import;

namespace JiraTools.Timesheet;

internal readonly struct ClockifyTaskName
{
    public const char Separator = ':';

    public string Key { get; }

    public string? Hint { get; }

    private ClockifyTaskName(string key, string? hint)
    {
        Key = key;
        Hint = hint;
    }

    public static Result<ClockifyTaskName> Parse(string value)
    {
        if (value is null)
        {
            return Result.Failure<ClockifyTaskName>(Res.ClockifyTaskName_CannotBeNull);
        }

        var separatorIndex = value.IndexOf(Separator);
        var valueSpan = value.AsSpan();
        if (separatorIndex < 0)
        {
            return Result.Success(new ClockifyTaskName(value.Trim(), null));
        }
        if (separatorIndex == 0)
        {
            return Result.Failure<ClockifyTaskName>(Res.ClockifyTaskName_KeyCannotBeEmpty);
        }

        var key = valueSpan[..separatorIndex].Trim().ToString();
        string? hint = separatorIndex < value.Length - 1
            ? valueSpan[(separatorIndex + 1)..].Trim().ToString()
            : null;
        return Result.Success(new ClockifyTaskName(key, hint));
    }
}