using CrystaLearn.Core.Models.Identity;
using CrystaLearn.Core.Models.Infra;

namespace CrystaLearn.Core.Models.Crysta;

public class CrystaTaskComment : Entity
{
    [MaxLength(200)]
    public string ProviderTaskId { get; set; }

    [MaxLength(200)]
    public string? ProviderCommentId { get; set; }

    // Thread identifiers for threaded discussions
    public int? ThreadId { get; set; }
    public int? ParentCommentId { get; set; }

    // Comment content
    public string? Text { get; set; }
    public string? FormattedText { get; set; }

    // Author information
    [MaxLength(255)]
    public string? CreatedByText { get; set; }

    public User? CreatedBy { get; set; }

    public DateTimeOffset? CreatedDateTime { get; set; }

    public DateTimeOffset? EditedDateTime { get; set; }

    [MaxLength(255)]
    public string? EditedByText { get; set; }

    public User? EditedBy { get; set; }

    // Status flags
    public bool IsDeleted { get; set; }
    public bool IsSystem { get; set; }

    // Comment metadata
    [MaxLength(100)]
    public string? CommentType { get; set; }

    [MaxLength(100)]
    public string? Visibility { get; set; }

    // Raw data
    public string? RawJson { get; set; }

    [MaxLength(1000)]
    public string? ProviderCommentUrl { get; set; }

    public string? Reactions { get; set; }

    public string Revision { get; set; }

    // Relationships
    public Guid CrystaTaskId { get; set; }
    public CrystaTask CrystaTask { get; set; } = default!;

    // Legacy fields for backward compatibility
    public User? User { get; set; }
    public SyncInfo? SyncInfo { get; set; }
    public CrystaProgram? CrystaProgram { get; set; }
    
    public string? Content { get; set; }
    public string? ContentHtml { get; set; }
}
