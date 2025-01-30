namespace CrystaLearn.Server.Api.Models.Crysta;

[ComplexType]
public class SyncInfo
{
    public Guid SyncId { get; set; }
    public DateTimeOffset SyncStartDateTime { get; set; }
    public DateTimeOffset SyncEndDateTime { get; set; }
    [MaxLength(100)]
    public string? SyncHash { get; set; }
    public SyncStatus? SyncStatus { get; set; }
}
