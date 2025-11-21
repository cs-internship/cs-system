using CrystaLearn.Core.Models.Identity;
using CrystaLearn.Core.Models.Infra;

namespace CrystaLearn.Core.Models.Crysta;

public class CrystaTaskUpdate : Entity
{
    [MaxLength(200)]
    public string? ProviderUpdateId { get; set; }

    [MaxLength(200)]
    public string? ProviderTaskId { get; set; }

    // Revision number where this update occurred
    public string Revision { get; set; }

    // Actor information
    [MaxLength(255)]
    public string? ChangedBy { get; set; }

    [MaxLength(200)]
    public string? ChangedById { get; set; }

    public DateTimeOffset ChangedDate { get; set; }

    // Field change details
    [MaxLength(255)]
    public string? FieldName { get; set; }

    [MaxLength(255)]
    public string? FieldDisplayName { get; set; }

    public string? OldValue { get; set; }
    public string? NewValue { get; set; }

    [MaxLength(100)]
    public string? Operation { get; set; }

    // Comment attached to update
    public string? CommentText { get; set; }

    // Flags
    public bool IsWorkItemFieldChange { get; set; }

    // Additional change data
    public string? ChangedPropertiesJson { get; set; }
    public string? RelationChange { get; set; }
    public string? AttachmentChange { get; set; }

    // Provider URL
    [MaxLength(1000)]
    public string? ProviderUrl { get; set; }

    // Raw data
    public string? RawJson { get; set; }

    // Relationships
    public Guid CrystaTaskId { get; set; }
    public CrystaTask CrystaTask { get; set; } = default!;

    // Legacy fields for backward compatibility
    public User? User { get; set; }
    public SyncInfo? SyncInfo { get; set; }
    public CrystaProgram? CrystaProgram { get; set; }
}
