namespace Clockify;

public sealed class ClockifyOptions
{
    public const string SectionName = "Clockify";

    public required string ApiBaseUrl { get; set; }
    public required string ApiKey { get; set; }
}