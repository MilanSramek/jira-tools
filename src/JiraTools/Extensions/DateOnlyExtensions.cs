namespace JiraTools.Extensions;

internal static class DateOnlyExtensions
{
    public static (DateOnly StartOfDay, DateOnly EndOfDay) GetSurroundingDayDateRange(
        this DateOnly anchorDate)
    {
        return (anchorDate, anchorDate);
    }

    public static (DateOnly StartOfWeek, DateOnly EndOfWeek) GetSurroundingWeekDateRange(
        this DateOnly anchorDate)
    {
        var dayOfWeek = (int)anchorDate.DayOfWeek;
        if (dayOfWeek < 0)
        {
            dayOfWeek = 7;
        }

        return (anchorDate.AddDays(1 - dayOfWeek), anchorDate.AddDays(7 - dayOfWeek));
    }

    public static (DateOnly StartOfMonth, DateOnly EndOfMonth) GetSurroundingMonthDateRange(
        this DateOnly anchorDate)
    {
        var year = anchorDate.Year;
        var month = anchorDate.Month;
        var lastDay = DateTime.DaysInMonth(year, month);

        return (new DateOnly(year, month, 1), new DateOnly(year, month, lastDay));
    }
}