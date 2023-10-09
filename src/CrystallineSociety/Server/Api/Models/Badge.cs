using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Server.Api.Models;

public class Badge
{
    [Key]
    public virtual required Guid Id { get; set; }
    public virtual required string Code { get; set; }
    public virtual required string Title { get; set; }
    public virtual string? Description { get; set; }
    public virtual string? Prerequisites { get; set; }
    public virtual string? PrerequisitesJsonSourceUrl { get; set; }
    public virtual string? PrerequisitesJson { get; set; }
    public virtual BadgeLevel Level { get; set; }
    public virtual bool IsApprovalRequired { get; set; }


    [ForeignKey(nameof(EducationProgramId))]
    public virtual EducationProgram? EducationProgram { get; set; }

    public virtual required Guid EducationProgramId { get; set; }
}
