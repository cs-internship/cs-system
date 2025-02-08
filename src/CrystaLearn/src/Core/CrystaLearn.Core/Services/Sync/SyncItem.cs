using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Services.Sync;

public class SyncItem
{
    public Guid? Id { get; set; }
    public SyncInfo SyncInfo { get; set; } = default!;
}
