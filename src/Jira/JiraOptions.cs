namespace Jira;

public sealed class JiraOptions
{
    public const string SectionName = "Jira";

    public required string ApiBaseUrl { get; set; }
    public required string Email { get; set; }
    public required string ApiKey { get; set; }
}
