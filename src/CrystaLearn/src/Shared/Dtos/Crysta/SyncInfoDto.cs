using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystaLearn.Shared.Dtos.Crysta;
public class SyncInfoDto
{
    public Guid SyncId { get; set; }
    public DateTimeOffset SyncStartDateTime { get; set; }
    public DateTimeOffset SyncEndDateTime { get; set; }
    [MaxLength(100)]
    public string? SyncHash { get; set; }
    public SyncStatus? SyncStatus { get; set; }
}
