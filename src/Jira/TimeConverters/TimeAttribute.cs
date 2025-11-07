namespace Jira.TimeConverters;

/// <summary>
/// Indicates that a DateTime parameter should be serialized as ISO8601 format in URLs.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
internal sealed class TimeAttribute(string format) : Attribute
{
    public string Format => format ?? throw new ArgumentNullException(nameof(format));
}
