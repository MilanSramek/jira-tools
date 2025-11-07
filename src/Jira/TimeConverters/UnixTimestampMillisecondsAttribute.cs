namespace Jira.TimeConverters;

/// <summary>
/// Attribute to mark DateTime parameters that should be serialized as Unix timestamp in milliseconds.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
internal sealed class UnixTimestampMillisecondsAttribute : Attribute
{
}
