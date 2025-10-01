using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystaLearn.Shared.Dtos.Crysta;
public class CrystaProgramDto
{
    public Guid Id { get; set; }
    public virtual required string Code { get; set; }
    public virtual required string Title { get; set; }
    public virtual string? BadgeSystemUrl { get; set; }
    public virtual string? DocumentUrl { get; set; }
    public virtual SyncInfoDto DocumentSyncInfo { get; set; } = new();
    public virtual SyncInfoDto BadgeSyncInfo { get; set; } = new();
    public virtual bool IsActive { get; set; }
}
