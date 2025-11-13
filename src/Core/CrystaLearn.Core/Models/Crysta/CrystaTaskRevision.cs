using CrystaLearn.Core.Models.Infra;

namespace CrystaLearn.Core.Models.Crysta;

public class CrystaTaskRevision : Entity
{
    // Azure Work Item ID this revision belongs to
    public int AzureWorkItemId { get; set; }

    // Revision number
    public int RevisionNumber { get; set; }

    // Snapshot data
    public string? SnapshotJson { get; set; }

    // Key fields at this revision
    [MaxLength(500)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [MaxLength(100)]
    public string? State { get; set; }

    // Actor who caused the revision
    [MaxLength(255)]
    public string? ChangedBy { get; set; }

    public DateTimeOffset ChangedDate { get; set; }

    // Original creation date
    public DateTimeOffset CreatedDate { get; set; }

    // Project identifiers
    public Guid ProjectId { get; set; }

    [MaxLength(255)]
    public string? ProjectName { get; set; }

    // Snapshot data
    public string? FieldsSnapshot { get; set; }
    public string? RelationsSnapshot { get; set; }
    public string? AttachmentsSnapshot { get; set; }
    public string? CommentsSnapshot { get; set; }

    // Change summary
    [MaxLength(1000)]
    public string? ChangeSummary { get; set; }

    public string? ChangeDetails { get; set; }

    // Link to the update that produced this revision
    public int? CreatedFromUpdateId { get; set; }

    // Raw JSON (alias to SnapshotJson)
    public string? RawJson { get; set; }

    // Relationships
    public Guid CrystaTaskId { get; set; }
    public CrystaTask CrystaTask { get; set; } = default!;

    // Legacy field for backward compatibility
    [MaxLength(100)]
    public string? RevisionCode { get; set; }
}
