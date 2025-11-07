using System.Reflection;

using Refit;

namespace Clockify.TimeConverters;

internal sealed class ISO8601UrlParameterFormatter : DefaultUrlParameterFormatter
{
    public override string? Format(object? value, ICustomAttributeProvider attributeProvider, Type type)
    {
        if (value is DateTime dateTime)
        {
            var utcValue = dateTime.Kind == DateTimeKind.Utc
                ? dateTime
                : dateTime.ToUniversalTime();
            return utcValue.ToString(ISO8601TimeFormat.DateTimeFormat);
        }

        return base.Format(value, attributeProvider, type);
    }
}
