namespace Clockify;

public sealed record UserQueryParameters
{
    public string? Email { get; init; }
    public string? Name { get; init; }
    public int? Page { get; init; } = 1;
    public int? PageSize { get; init; } = 50;
    public string? Status { get; init; }
    public string? MembershipsTargetId { get; init; }
    public string? MembershipsStatus { get; init; }
    public bool? IncludeRoles { get; init; }
}