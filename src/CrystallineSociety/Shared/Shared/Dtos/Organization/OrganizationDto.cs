namespace CrystallineSociety.Shared.Dtos.Organization;
public class OrganizationDto
{
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string BadgeSystemUrl { get; set; } = default!;
    public string ProgramDocumentUrl { get; set; } = default!;
    public DateTimeOffset LastSyncDateTime { get; set; }
    public string? LastCommitHash { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
