using System.Collections.ObjectModel;

namespace CrystallineSociety.Server.Api.Models;

public class EducationProgram
{
    [Key]
    public virtual required Guid Id { get; set; }
    public virtual required string Code { get; set; }
    public virtual required string Title { get; set; }
    public virtual required string BadgeSystemUrl { get; set; }
    public virtual DateTimeOffset LastSyncDateTime { get; set; }
    public virtual string? LastCommitHash { get; set; }
    public virtual bool IsActive { get; set; }

    public virtual ObservableCollection<Badge>? Badges { get; set; }
}
