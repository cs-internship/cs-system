namespace CrystaLearn.Core.Models.Crysta;

[ComplexType]
public class SyncInfo
{
    /// <summary>
    /// For relation between our system and 3rd party system
    /// </summary>
    [MaxLength(100)]
    public string? SyncId { get; set; }
    public DateTimeOffset? SyncStartDateTime { get; set; }
    public DateTimeOffset? SyncEndDateTime { get; set; }

    [MaxLength(100)]
    public string? ContentHash { get; set; } = string.Empty;

    /// <summary>
    /// Azure board or Manual
    /// </summary>
    [MaxLength(100)]
    public string? SyncGroup { get; set; } = string.Empty;

    public SyncStatus? SyncStatus { get; set; }
    public DateTimeOffset? LastSyncDateTime { get; set; }

    [MaxLength(40)]
    public string? LastSyncOffset { get; set; }

}
