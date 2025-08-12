namespace CrystaLearn.Core.Models.Crysta;

[ComplexType]
public class SyncInfo
{
    [MaxLength(100)]
    public string? SyncId { get; set; }
    public DateTimeOffset? SyncStartDateTime { get; set; }
    public DateTimeOffset? SyncEndDateTime { get; set; }
    [MaxLength(100)]
    public string? SyncHash { get; set; } = string.Empty;
    public SyncStatus? SyncStatus { get; set; }
    public DateTimeOffset? LastSyncDateTime { get; set; }
    [MaxLength(40)]
    public string? LastSyncOffset { get; set; }

}
