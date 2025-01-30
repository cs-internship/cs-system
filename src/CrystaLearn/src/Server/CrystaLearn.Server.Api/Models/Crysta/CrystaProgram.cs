using System.Collections.ObjectModel;
using CrystaLearn.Server.Api.Models.Infra;

namespace CrystaLearn.Server.Api.Models.Crysta;

public class CrystaProgram : Entity
{
    [MaxLength(50)]
    public virtual required string Code { get; set; }
    [MaxLength(200)]
    public virtual required string Title { get; set; }
    [MaxLength(300)]
    public virtual string? BadgeSystemUrl { get; set; }
    [MaxLength(300)]
    public virtual string? DocumentUrl { get; set; }
    public virtual SyncInfo DocumentSyncInfo { get; set; } = new();
    public virtual SyncInfo BadgeSyncInfo { get; set; } = new();
    public virtual bool IsActive { get; set; }
}
